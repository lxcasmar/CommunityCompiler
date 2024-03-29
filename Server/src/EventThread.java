import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.text.DateFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.UUID;
import org.json.JSONArray;
import org.json.JSONObject;


public class EventThread extends Thread{
    private final Socket socket;
    private EventServer es;
    
    public EventThread(Socket _socket, EventServer _my_ev) {
        this.socket = _socket;
        this.es = _my_ev;
    }

    public void run() {
        try {
            System.out.println("*** New connection from " + socket.getInetAddress() + ":" + socket.getPort() + " ***");
            InputStream input = socket.getInputStream();
            OutputStream output = socket.getOutputStream();
            Boolean proceed = true;

            RFC6455.Handshake(input, output);

            do {
                byte[] buffer = new byte[256];
                int bytesRead;
                StringBuilder message = new StringBuilder();
                while ((bytesRead = input.read(buffer)) != -1) {
                    try {
                        message.append(new String(RFC6455.decode(buffer)));
                    } catch (Exception e) {
                        e.printStackTrace();
                    }
                    if (input.available() == 0) {
                        break;
                    }
                }
                
                String cleanedMessage = message.toString().trim();
                String tag = message.toString().split(";")[0];
                String response = tag + "\n";
                //System.out.println("Received message" + cleanedMessage);
                System.out.println("Received request: " + tag);
                switch (tag) {
                    case "HELLO":
                        response += "ResponseToHello";
                        break;
                    case "CRTEVT":
                        String [] args = cleanedMessage.split(";")[1].split(Server.PARAM_DELIMITER);
                        response += Boolean.toString(createEvent(args));
                        break;
                    case "ALLEVT":
                        response += selectAll();
                        break;
                    case "SELEVT":
                        response += selectEvent(cleanedMessage.split(";")[1]);
                        break;
                    case "SRCHEVT":
                        args = cleanedMessage.split(";")[1].split(Server.PARAM_DELIMITER);
                        response += searchEvent(args[0], args[1]);
                        break;
                    default:
                        response += "Unknown request";
                        break;
                }

                output.write(RFC6455.encode(response, false));
            } while (proceed);
        } catch (IOException e) {
            System.err.println("Error: " + e.getMessage());
            e.printStackTrace();
        } finally {
            System.out.println("*** Connection from " + socket.getInetAddress() + ":" + socket.getPort() + " closed ***");
        }
    }

    public static boolean seatGeekCrtEvt(String[] args) {
        String sql = "INSERT INTO events (uuid, owner, name, description, location, startDate, endDate, url, type, image) VALUES (?,?,?,?,?,?,?,?,?,?)";
        String uuid = UUID.randomUUID().toString();
        String title = args[0];
        String location = args[1];
        String eventUrl = args[2];
        String description = args[3];
        String type = args[4];
        String image = args[5];
        String datetime = args[6];
        String endDatetime = args[7];
        String owneruuid = args[8];
        try (Connection conn = DriverManager.getConnection(Server.dbUrl)) {
            PreparedStatement pstmt = conn.prepareStatement(sql);
            pstmt.setString(1, uuid);
            pstmt.setString(2, owneruuid);
            pstmt.setString(3, title);
            pstmt.setString(4, description);
            pstmt.setString(5, location);

            DateTimeFormatter formatter = DateTimeFormatter.ISO_LOCAL_DATE_TIME;
            LocalDateTime utcDateTime = LocalDateTime.parse(datetime, formatter);
            java.sql.Date date = java.sql.Date.valueOf(utcDateTime.toLocalDate());

            pstmt.setDate(6, date);

            if (endDatetime != null) {
                utcDateTime = LocalDateTime.parse(endDatetime, formatter);
                date = java.sql.Date.valueOf(utcDateTime.toLocalDate());
                pstmt.setDate(7, date);
            } else {
                pstmt.setDate(7, null);
            }

            pstmt.setString(8,eventUrl);
            pstmt.setString(9,type);
            pstmt.setString(10, image);
            pstmt.executeUpdate();
            return true;

        } catch (SQLException e) {
            System.out.println(e.getMessage());
        }
        
        return true;
    }

    // TODO: add way to insert image... maybe by uploading to server and storing path in db?
    private boolean createEvent(String [] args) {
        String endDate = null, owneruuid, name, description, location, startDate, sql = null;
        System.out.println("args length: " + args.length);
        switch(args.length) {
            case 6:
                endDate = args[5];
                sql = "INSERT INTO events (uuid, owner, name, description, location, startDate, endDate, image, url, type) VALUES (?,?,?,?,?,?,?,?,?,?)";
            case 5:
                if (sql == null) {
                    sql = "INSERT INTO events (uuid, owner, name, description, location, startDate, image, url, type) VALUES (?,?,?,?,?,?,?,?,?)";
                }
                owneruuid = args[0];
                name = args[1];
                description = args[2];
                location = args[3];
                startDate = args[4];
                break;
            default:
                return false;
        }
        String uuid = UUID.randomUUID().toString();
        try (Connection conn = DriverManager.getConnection(Server.dbUrl)) {
            PreparedStatement pstmt = conn.prepareStatement(sql);
            pstmt.setString(1, uuid);
            pstmt.setString(2, owneruuid);
            pstmt.setString(3, name);
            pstmt.setString(4, description);
            pstmt.setString(5, location);
            pstmt.setString(6, "");
            pstmt.setString(7, "");
            pstmt.setString(8, "");

            DateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
            java.util.Date parsedDate;
            try {
                parsedDate = dateFormat.parse(startDate);
            } catch (ParseException e) {
                e.printStackTrace();
                System.out.println("Date format was entered incorrectly");
                return false;
            }        
            pstmt.setDate(6, new java.sql.Date(parsedDate.getTime()));
            if (endDate != null) {
                try {
                    parsedDate = dateFormat.parse(endDate);
                } catch (ParseException e) {
                    e.printStackTrace();
                    System.out.println("Date format was entered incorrectly");
                    return false;
                }        
                pstmt.setDate(7, new java.sql.Date(parsedDate.getTime()));
            }

            pstmt.executeUpdate();
            return true;

        } catch (SQLException e) {
            System.out.println(e.getMessage());
        }
        return false;
    }

    private String selectAll() {
        String sql = "SELECT * FROM events";

        try (Connection conn = DriverManager.getConnection(Server.dbUrl);
                PreparedStatement pstmt = conn.prepareStatement(sql)) {

            ResultSet rs = pstmt.executeQuery();
            JSONArray rows = new JSONArray();
            while (rs.next()) {
                JSONObject row = new JSONObject();
                row.put("uuid", rs.getString("uuid"));
                row.put("owner", rs.getString("owner"));
                row.put("name", rs.getString("name"));
                row.put("description", rs.getString("description"));
                row.put("location", rs.getString("location"));
                row.put("startDate", rs.getString("startDate"));
                row.put("endDate", rs.getString("endDate"));
                row.put("image", rs.getString("image"));
                row.put("url", rs.getString("url"));
                row.put("type", rs.getString("type"));
                rows.put(row);
            }

            String json = rows.toString(4);
            return json;
        } catch (SQLException e) {
            System.out.println(e.getMessage());
        }
        return null;
    }

    private String selectEvent(String uuid) {
        String sql = "SELECT * FROM events WHERE uuid = ?";

        try (Connection conn = DriverManager.getConnection(Server.dbUrl);
                PreparedStatement pstmt = conn.prepareStatement(sql)) {

            pstmt.setString(1, uuid);
            ResultSet rs = pstmt.executeQuery();
            JSONArray rows = new JSONArray();
            JSONObject obj = new JSONObject();
            if (rs.next()) {
                obj.put("uuid", rs.getString("uuid"));
                obj.put("owner", rs.getString("owner"));
                obj.put("name", rs.getString("name"));
                obj.put("description", rs.getString("description"));
                obj.put("location", rs.getString("location"));
                obj.put("startDate", rs.getString("startDate"));
                obj.put("endDate", rs.getString("endDate"));
                obj.put("image", rs.getString("image"));
                obj.put("url", rs.getString("url"));
                obj.put("type", rs.getString("type"));
            }
            // while (rs.next()) {
            //     JSONObject row = new JSONObject();
            //     row.put("uuid", rs.getString("uuid"));
            //     row.put("owner", rs.getString("owner"));
            //     row.put("name", rs.getString("name"));
            //     row.put("description", rs.getString("description"));
            //     row.put("location", rs.getString("location"));
            //     row.put("startDate", rs.getString("startDate"));
            //     row.put("endDate", rs.getString("endDate"));
            //     row.put("image", rs.getString("image"));
            //     row.put("url", rs.getString("url"));
            //     rows.put(row);
            // }

            // String json = rows.toString(4);
            String json = obj.toString(4);
            return json;
        } catch (SQLException e) {
            System.out.println(e.getMessage());
        }
        return null;
    }

    private String searchEvent(String column, String value) {
        String sql = "SELECT * FROM events WHERE " + column + " LIKE " + value;

        try (Connection conn = DriverManager.getConnection(Server.dbUrl);
                PreparedStatement pstmt = conn.prepareStatement(sql)) {

            ResultSet rs = pstmt.executeQuery();
            JSONArray rows = new JSONArray();
            while (rs.next()) {
                JSONObject row = new JSONObject();
                row.put("uuid", rs.getString("uuid"));
                row.put("owner", rs.getString("owner"));
                row.put("name", rs.getString("name"));
                row.put("description", rs.getString("description"));
                row.put("location", rs.getString("location"));
                row.put("startDate", rs.getString("startDate"));
                row.put("endDate", rs.getString("endDate"));
                row.put("image", rs.getString("image"));
                row.put("url", rs.getString("url"));
                rows.put(row);
            }

            String json = rows.toString(4);
            return json;
        } catch (SQLException e) {
            System.out.println(e.getMessage());
        }
        return null;
    }
}
