import java.net.ServerSocket;
import java.net.Socket;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;
public class EventServer extends Server{
    
    public EventServer(int _port) {
        super(_port, "beta");
    }

    public EventServer(int _port, String host) {
        super(_port, host);
    }

    @Override
    void start() {
        Runtime runtime = Runtime.getRuntime();
        runtime.addShutdownHook(new ShutDownListener());

        // connect to the datebase and create a new events table if it doesn't exist
        ConnectToDatabase();
        CreateNewtable();

        try {
            final ServerSocket serverSocket = new ServerSocket(port);
            System.out.printf("%s up and running on port %d\n", this.getClass().getName(), port);

            Socket sock = null;
            EventThread thread = null;

            while (true) {
                sock = serverSocket.accept();
                thread = new EventThread(sock, this);
                thread.start();
            }
        } catch (Exception e) {
            System.err.println("Error: " + e.getMessage());
            e.printStackTrace(System.err);
        }
    }

    @Override
    void CreateNewtable() {
        String sql = "CREATE TABLE IF NOT EXISTS events (\n"
                + "	uuid text PRIMARY KEY,\n"
                + "	name text NOT NULL,\n"
                + "	description text NOT NULL,\n"
                + " startDate datetime NOT NULL,\n"
                + "	location text NOT NULL,\n"
                + " endDate datetime,\n"
                + " capacity integer,\n"
                + "	owner text NOT NULL,\n"
                + "	FOREIGN KEY (owner) REFERENCES users(uuid)\n"
                + ");";
        try (Connection conn = DriverManager.getConnection(dbUrl)) {
            conn.createStatement().execute(sql);
        } catch (SQLException e) {
            System.out.println(e.getMessage());
        }
    }
}
