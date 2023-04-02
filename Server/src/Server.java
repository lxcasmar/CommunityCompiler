import java.net.InetAddress;
import java.net.UnknownHostException;
import java.sql.Connection;
import java.sql.DatabaseMetaData;
import java.sql.DriverManager;
import java.sql.SQLException;

public abstract class Server {
    protected int port;
    public String host;
    abstract void start();
    static final String dbUrl = "jdbc:sqlite:db/DB.db";
    public static final String PARAM_DELIMITER = "##";

    public Server(int _SERVER_PORT, String _SERVER_NAME) {
        port = _SERVER_PORT;
        host = _SERVER_NAME;
    }

    public Server(int _SERVER_PORT) {
        port = _SERVER_PORT;
        InetAddress inetAddress;
        try {
            inetAddress = InetAddress.getLocalHost();
            String ipAddress = inetAddress.getHostAddress();
            host = ipAddress;
        } catch (UnknownHostException e) {
            e.printStackTrace();
        }
    }

    public int GetPort() {
        return port;
    }

    public String GetName() {
        return host;
    }
    abstract void CreateNewtable();
    
    public static void ConnectToDatabase() {
        try (Connection conn = DriverManager.getConnection(dbUrl)) {
            if (conn != null) {
                DatabaseMetaData meta = conn.getMetaData();
                System.out.println("The driver name is " + meta.getDriverName());
                System.out.println("A new database has been created.");
            }
        } catch (SQLException e) {
            System.out.println(e.getMessage());
        }
    }
}
