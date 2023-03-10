import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.security.SecureRandom;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.UUID;
import org.json.JSONArray;
import org.json.JSONObject;
import org.bouncycastle.util.encoders.Hex;

public class UserThread extends Thread{
    private final Socket socket;
    private UserServer my_us; // check if this will be needed?

    public UserThread(Socket _socket, UserServer _us) {
        this.socket = _socket;
        this.my_us = _us;
    }

    public void run() {
        try {
            System.out.println("*** New connection from " + socket.getInetAddress() + ":" + socket.getPort() + " ***");
            InputStream input = socket.getInputStream();
            OutputStream output = socket.getOutputStream();
            Boolean proceed = true;
            do {
                // read message from client
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
                    case "CRTUSR":
                        String[] args = message.toString().split(";")[1].split(Server.PARAM_DELIMITER);
                        response = Boolean.toString(createUser(args[0], args[1], args[2], args[3], args[4]));
                        break;
                    case "ALLUSR":
                        response = selectAll();
                        break;
                    case "SELUSR":
                        response = selectUser(message.toString().split(";")[1]);
                        break;
                    case "AUTH":
                        args = message.toString().split(";")[1].split(Server.PARAM_DELIMITER);
                        response = Boolean.toString(auth(args[0], args[1]));
                        break;
                    default:
                        response = "Unknown request received";
                        break;
                }
                
                output.write(response.getBytes());
            } while (proceed);
        } catch (IOException e) {
            System.err.println("Error: " + e.getMessage());
            e.printStackTrace(System.err);
        }
    }

    private boolean createUser(String username, String password, String email, String phone, String isAdmin) {
        String uuid = UUID.randomUUID().toString();
        String sql = "INSERT INTO users (uuid, username, email, password, salt, phone, is_admin) VALUES (?, ?, ?, ?, ?, ?, ?)";
        // need to check if username already exists
        try (Connection conn = DriverManager.getConnection(Server.dbUrl)){
            PreparedStatement pstmt = conn.prepareStatement(sql);
            pstmt.setString(1, uuid);
            pstmt.setString(2, username);
            pstmt.setString(3, email);

            byte[] salt = new byte[BCRYPT.SALT_LENGTH];
            new SecureRandom().nextBytes(salt);
            String hashedPassword = BCRYPT.Hash(password, salt);

            pstmt.setString(4, hashedPassword);
            pstmt.setString(5, Hex.toHexString(salt));
            pstmt.setString(6, phone);
            pstmt.setInt(7, Integer.parseInt(isAdmin));
            pstmt.executeUpdate();
            return true;
        } catch (SQLException e) {
            System.out.println(e.getMessage());
        }
        return false;
    }

    private String selectAll() {
        String sql = "SELECT uuid, username, email, password, phone FROM users";

        try (Connection conn = DriverManager.getConnection(Server.dbUrl);
                PreparedStatement pstmt = conn.prepareStatement(sql)) {

            ResultSet rs = pstmt.executeQuery();
            JSONArray rows = new JSONArray();
            // loop through the result set
            while (rs.next()) {
                // create a JSONObject for each row and add the values
                JSONObject row = new JSONObject();
                row.put("uuid", rs.getString("uuid"));
                row.put("username", rs.getString("username"));
                row.put("email", rs.getString("email"));
                row.put("password", rs.getString("password"));
                row.put("phone", rs.getString("phone"));
                rows.put(row);
            }

            String json = rows.toString(4);
            return json;
        } catch (SQLException e) {
            System.out.println(e.getMessage());
        }
        return null;
    }

    private String selectUser(String uuid) {
        String sql = "SELECT uuid, username, email, password, phone FROM users WHERE uuid = ?";

        try (Connection conn = DriverManager.getConnection(Server.dbUrl);
                PreparedStatement pstmt = conn.prepareStatement(sql)) {

            pstmt.setString(1, uuid);
            ResultSet rs = pstmt.executeQuery();
            JSONArray rows = new JSONArray();
            // loop through the result set
            while (rs.next()) {
                // create a JSONObject for each row and add the values
                JSONObject row = new JSONObject();
                row.put("uuid", rs.getString("uuid"));
                row.put("username", rs.getString("username"));
                row.put("email", rs.getString("email"));
                row.put("password", rs.getString("password"));
                row.put("phone", rs.getString("phone"));
                rows.put(row);
            }

            String json = rows.toString(4);
            return json;
        } catch (SQLException e) {
            System.out.println(e.getMessage());
        }
        return null;
    }    

    private boolean auth(String username, String password) {
        String sql = "SELECT uuid, username, email, password, salt, phone FROM users WHERE username = ?";

        try (Connection conn = DriverManager.getConnection(Server.dbUrl);
                PreparedStatement pstmt = conn.prepareStatement(sql)) {

            pstmt.setString(1, username);
            ResultSet rs = pstmt.executeQuery();
            String dbPassword;
            String dbSalt;
            if (rs.next()) {
                dbPassword = rs.getString("password");
                dbSalt = rs.getString("salt");
            } else {
                // no user with that username
                return false;
            }
            
            return BCRYPT.Check(password, dbPassword, Hex.decode(dbSalt));
        } catch (SQLException e) {
            System.out.println(e.getMessage());
        }
        return false;        
    }
}
