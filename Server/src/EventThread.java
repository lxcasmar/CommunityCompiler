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
            do {
                byte[] buffer = new byte[256];
                int bytesRead;
                StringBuilder message = new StringBuilder();
                while ((bytesRead = input.read(buffer)) != -1) {
                    message.append(new String(buffer, 0, bytesRead));
                    if (input.available() == 0) {
                        break;
                    }
                }
                String response;
                String tag = message.toString().split(";")[0];
                //String args = message.toString().split(":")[1];
                System.out.println("Received request: " + tag);

                switch (tag) {
                    case "HELLO":
                        response = "ResponseToHello";
                        break;
                    case "CRTEVT":
                        String [] args = message.toString().split(";")[1].split(Server.PARAM_DELIMITER);
                        response = Boolean.toString(createEvent(args));
                        break;
                    case "ALLEVT":
                        response = selectAll();
                        break;
                    case "SELEVT":
                        response = selectEvent(message.toString().split(";")[1]);
                        break;
                    case "SRCHEVT":
                        args = message.toString().split(";")[1].split(Server.PARAM_DELIMITER);
                        response = searchEvent(args[0], args[1]);
                        break;
                    default:
                        response = "Unknown request";
                        break;
                }

                output.write(response.getBytes());
            } while (proceed);
        } catch (IOException e) {
            System.err.println("Error: " + e.getMessage());
            e.printStackTrace();
        }
    }

    private boolean createEvent(String [] args) {
        String endDate = null, capacity = null, owneruuid, name, description, location, startDate, sql = null;
        System.out.println("args length: " + args.length);
        switch(args.length) {
            case 7:
                endDate = args[5].substring(1);
                capacity = args[6].substring(1);
                sql = "INSERT INTO events (uuid, owner, name, description, location, startDate, endDate, capacity) VALUES (?,?,?,?,?,?,?,?)";
            case 6:
                if(args[6].charAt(0) == 'A') {
                    endDate = args[6].substring(1);
                    sql = "INSERT INTO events (uuid, owner, name, description, location, startDate, endDate) VALUES (?,?,?,?,?,?,?)";
                } else {
                    capacity = args[6].substring(1);
                    sql = "INSERT INTO events (uuid, owner, name, description, location, startDate, capacity) VALUES (?,?,?,?,?,?,?)";
                }
            case 5:
                if (sql == null) {
                    sql = "INSERT INTO events (uuid, owner, name, description, location, startDate) VALUES (?,?,?,?,?,?)";
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

            if (capacity != null) {
                pstmt.setInt(7, Integer.parseInt(capacity));
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
                row.put("capacity", rs.getString("capacity"));
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
            while (rs.next()) {
                JSONObject row = new JSONObject();
                row.put("uuid", rs.getString("uuid"));
                row.put("owner", rs.getString("owner"));
                row.put("name", rs.getString("name"));
                row.put("description", rs.getString("description"));
                row.put("location", rs.getString("location"));
                row.put("startDate", rs.getString("startDate"));
                row.put("endDate", rs.getString("endDate"));
                row.put("capacity", rs.getString("capacity"));
                rows.put(row);
            }

            String json = rows.toString(4);
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
                row.put("capacity", rs.getString("capacity"));
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
