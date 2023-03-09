import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;

/**
 * This class and its subclasses are for test purposes only.
 * The actual client is the mobile application built with .NET.
 */
public abstract class Client {
    protected Socket sock;
    protected InputStream input;
    protected OutputStream output;

    public boolean connect(final String server, final int port) {
        System.out.println("Connecting to " + server + ":" + port);
        try {
            sock = new Socket(server, port);
            input = sock.getInputStream();
            output = sock.getOutputStream();

            output.flush();
            return true;
        } catch (IOException e) {
            System.err.println("Error: " + e.getMessage());
            e.printStackTrace(System.err);
            return false;
        }
    }

    public boolean isConnected() {
        if (sock != null || !sock.isConnected()) {
            return false;
        } else {
            return true;
        }
    }

    public void disconnect() {
        if (isConnected()) {
            try {
                sock.close();
            } catch (IOException e) {
                System.err.println("Error: " + e.getMessage());
                e.printStackTrace(System.err);
            }
        }
    }
}