import java.sql.Connection;
import java.sql.DatabaseMetaData;
import java.sql.DriverManager;
import java.sql.SQLException;

public abstract class Server {
    protected int port;
    public String name;
    abstract void start();
    static String dbUrl = "jdbc:sqlite:db/DB.db";

    public Server(int _SERVER_PORT, String _SERVER_NAME) {
        port = _SERVER_PORT;
        name = _SERVER_NAME;
    }

    public int GetPort() {
        return port;
    }

    public String GetName() {
        return name;
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
