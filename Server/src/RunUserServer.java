public class RunUserServer {
    public static void main(String[] args) {
        java.security.Security.addProvider(new org.bouncycastle.jce.provider.BouncyCastleProvider());
        UserServer server;
        int port = -1;
        String host = null;

        switch(args.length) {
            case 1: 
                try {
                    port = Integer.parseInt(args[0]);
                } catch (NumberFormatException e) {
                    System.out.println("Enter valid port number or no port number for random selection");
                }
                break;
            case 2:
                try {
                    port = Integer.parseInt(args[0]);
                    host = args[1];
                } catch (NumberFormatException e) {
                    System.out.println("Enter valid port number or no port number for random selection");
                }
                break;
            default:
                java.util.Random rand = new java.util.Random();
                port = rand.nextInt(5000) + 10000;
                break;
        }

        System.out.println("Starting server on port " + port);
        server = host == null? new UserServer(port) : new UserServer(port, host);
        server.start();
    }
}