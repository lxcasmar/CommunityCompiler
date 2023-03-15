import java.util.Arrays;
import java.util.Scanner;

/**
 * This class is meant as a test for User Server/Client and Event Server/Client
 */
public class App {
    static UserClient uc;
    static EventClient ec;

    static String US_NAME = null;
    static String ES_NAME = null;
    static int US_PORT;
    static int ES_PORT;
    static Scanner sc;
    // format for dates is YYYY-MM-DD HH:MM:SS
    //technically all the '##' should be replaced by Server.PARAM_DELIMITER
    static String _HelpMsg = "- HELP\t\t\t Displays this help message\n" + 
                             "- HELLO\t\t\t Pings both servers to check response/connectivity\n" +
                             "- AUTH;<username>##<password>\t Attempts to authenticate a user\n" +
                             "- SELUSR;<UUID>\t\t Gets user information from UUID\n" +
                             "- SELEVT;<UUID>\t\t Gets event information from UUID\n" +
                             "- CRTUSR;<username>##<password>##<email>##<phoneNumber>##<is_admin>\t Creates a new user\n" +
                             "- CRTEVT;<owneruuid>##<name>##<description>##<location>##<start>##[<end>]##[<capacity>]    Creates a new event. Time in YYYY-MM-DD HH:MM:SS\n" +
                             "- SRCHEVT;<column>##<value>\t\t Searches for an event based on certain parameters\n" +
                             "- ALLUSR\t\t\t returns list of all users in the database\n" +
                             "- ALLEVT\t\t\t returns list of all events in the database";

    private static void usg_exit() {
        System.err.println("Usage: java App <US_NAME> <US_PORT> <ES_NAME> <ES_PORT>");
        System.exit(-1);
    }

    private static void connect() {
        String US_Host = US_NAME != null ? US_NAME : "localhost";
        String ES_Host = ES_NAME != null ? ES_NAME : "localhost";
        if (!uc.connect(US_Host, US_PORT)) {
            System.err.println("UserServer could not connect to " + US_Host + ":" + US_PORT);
            System.exit(-1);
        } else {
            System.out.println("UserServer connected to " + US_Host + ":" + US_PORT);
        }

        if (!ec.connect(ES_Host, ES_PORT)) {
            System.err.println("EventServer could not connect to " + ES_Host + ":" + ES_PORT);
            System.exit(-1);
        } else {
            System.out.println("EventServer connected to " + ES_Host + ":" + ES_PORT);
        }
    }

    private static void disconnect() {
        uc.disconnect();
        ec.disconnect();
        System.exit(1);
    }

    public static synchronized void main (String [] args) {
        java.security.Security.addProvider(new org.bouncycastle.jce.provider.BouncyCastleProvider());
        // two cases: 1) <us_port> <es_port> 2) <us_name> <us_port> <es_name> <es_port>
        System.out.println("**** REMEMBER... THIS APP AND THE CLIENT CLASSES MEANT FOR TESTING ***");
        if (args.length < 2) {
            usg_exit();
        } else if (args.length == 2) {
            try {
                US_PORT = Integer.parseInt(args[0]);
                ES_PORT = Integer.parseInt(args[1]);
            } catch (NumberFormatException e) {
                System.err.println("Invalid port number");
                usg_exit();
            }
        } else if (args.length == 4) {
            US_NAME = args[0];
            ES_NAME = args[2];
            try {
                US_PORT = Integer.parseInt(args[1]);
                ES_PORT = Integer.parseInt(args[3]);
            } catch (NumberFormatException e) {
                System.err.println("Invalid port number");
                usg_exit();
            }
        } else {
            usg_exit();
        }
        
        // initialize and connect the clients
        uc = new UserClient();
        ec = new EventClient();
        connect();

        // begin operations
        sc = new Scanner(System.in);
        String input = "", tag = "";
        while (!tag.equals("exit")) {
            System.out.println("Enter a command. HELP for help");
            input = sc.nextLine().trim();
            // do some edge case checking for commands with no args
            tag = input.split(";")[0].toUpperCase();
            System.out.println("READ TAG: " + tag);
            switch(tag) {
                case "HELLO":
                    System.out.println(uc.hello());
                    System.out.println(ec.hello());
                    break;
                case "HELP":
                    System.out.println(_HelpMsg);
                    break;
                case "CRTUSR":
                    try {
                        String [] params = input.split(";")[1].split("##");
                        if (!uc.createUser(params[0], params[1], params[2], params[3], params[4])) {
                            System.err.println("User creation failed");
                        }
                    } catch (Exception e) {
                        e.printStackTrace();
                    }
                    break;
                case "ALLUSR":
                    System.out.println(uc.selectAll());
                    break;
                case "SELUSR":
                    try {
                        String uuid = input.split(";")[1];
                        System.out.println(uc.selectUser(uuid));
                    } catch (Exception e) {
                        e.printStackTrace();
                    }
                    break;
                case "AUTH":
                    try {
                        String [] params = input.split(";")[1].split("##");
                        if (uc.auth(params[0], params[1])){
                            System.out.println("Authentication successful");
                        } else {
                            System.out.println("Authentication failed");
                        }
                            
                    } catch (Exception e) {
                        e.printStackTrace();
                    }
                    break;
                case "CRTEVT":
                    try {
                        String [] params = input.split(";")[1].split(Server.PARAM_DELIMITER);
                        Arrays.stream(params).forEach(x -> System.out.println(x));
                        
                        if (!crtevt(params)) {
                            System.err.println("Event creation failed");
                        } else {
                            System.out.println("Event creation successful");
                        }
                    } catch (Exception e) {
                        e.printStackTrace();
                    }
                    break;
                case "ALLEVT":
                    System.out.println(ec.selectAll());
                    break;
                case "SELEVT":
                    try {
                        String uuid = input.split(";")[1];
                        System.out.println(ec.selectEvent(uuid));
                    } catch (Exception e) {
                        e.printStackTrace();
                    }
                    break;
                case "SRCHEVT":
                    try {
                        String [] params = input.split(";")[1].split(Server.PARAM_DELIMITER);
                        System.out.println(ec.searchEvent(params[0], params[1]));
                    } catch (Exception e) {
                        e.printStackTrace();
                    }
                    break;
                case "EXIT":
                    disconnect();
                    break;
            }
        }
    }

    /**
     * Helper method for creating an event... needed because of optional parameters
     * @param args
     * @return
     */
    private static boolean crtevt(String [] args) {
        switch (args.length) {
            case 5:
                return ec.createEvent(args[0], args[1], args[2], args[3], args[4], null, null);
            case 6:
                return ec.createEvent(args[0], args[1], args[2], args[3], args[4], args[5], null);
            case 7:
                return ec.createEvent(args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
            default:
                System.err.println("Invalid number of arguments");
                return false;
        }
    }
}
