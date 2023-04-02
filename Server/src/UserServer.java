import java.net.ServerSocket;
import java.net.Socket;
import java.net.SocketException;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;


public class UserServer extends Server {

    public UserServer(int _port) {
        super(_port, "alpha");
    }

    public UserServer(int _port, String host) {
        super(_port, host);
    }

    @Override
    void start() {
        Runtime runtime = Runtime.getRuntime();
        runtime.addShutdownHook(new ShutDownListener());

        // connect to the datebase and create a new users table if it doesn't exist
        ConnectToDatabase();
        CreateNewtable();

        // listen to connections and create a thread on each new connection
        try {
            final ServerSocket serverSocket = new ServerSocket(port);
            System.out.printf("%s up and running on port %d\n", this.getClass().getName(), port);

            Socket sock = null;
            UserThread thread = null;

            while (true) {
                sock = serverSocket.accept();
                thread = new UserThread(sock, this);
                thread.start();
            }
        } catch (SocketException e) {
            System.out.println("SocketException: " + e.getMessage());
        } catch(Exception e) {
            System.err.println("Error: " + e.getMessage());
            e.printStackTrace(System.err);
        }
    }

    @Override
    void CreateNewtable() {
            
        String sql = "CREATE TABLE IF NOT EXISTS users (\n"
            + "	uuid text PRIMARY KEY,\n"
            + "	username text NOT NULL,\n"
            + "	email text NOT NULL,\n"
            + "	password text NOT NULL,\n"
            + " salt text NOT NULL,\n"
            + "	phone text NOT NULL,\n"
            + " is_admin integer NOT NULL\n"
            + ");";
        try (Connection conn = DriverManager.getConnection(dbUrl)){
            conn.createStatement().execute(sql);
        } catch (SQLException e) {
            System.out.println(e.getMessage());
        }
    }
}

class ShutDownListener extends Thread {
    public ShutDownListener() {}

    @Override
    public void run() {
        System.out.println("Shutting down server...");
    }
}
