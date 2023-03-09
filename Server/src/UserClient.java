public class UserClient extends Client{

    public String hello() {
        String message = "HELLO;restOfMessage";
        try {
            // send message to server
            output.write(message.getBytes());
            
            // read response from server
            byte[] buffer = new byte[256];
            int bytesRead;
            StringBuilder response = new StringBuilder();
            while ((bytesRead = input.read(buffer)) != -1) {
                response.append(new String(buffer, 0, bytesRead));
                if (input.available() == 0) {
                    break;
                }
            }

            return response.toString();
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        }
    }

    public boolean createUser(String username, String password, String email, String phone, String isAdmin) {
        String message = "CRTUSR;" + username + "##" + password + "##" + email + "##" + phone + "##" + isAdmin;
        try {
            System.out.println("Sending message: " + message);
            output.write(message.getBytes());

            // read response from server
            byte[] buffer = new byte[256];
            int bytesRead;
            StringBuilder response = new StringBuilder();
            while ((bytesRead = input.read(buffer)) != -1) {
                response.append(new String(buffer, 0, bytesRead));
                if (input.available() == 0) {
                    break;
                }
            }

            return Boolean.parseBoolean(response.toString());
        } catch (Exception e) {
            e.printStackTrace();
            return false;
        }
    }

    public String selectAll() {
        String message = "ALLUSR;";
        try {
            // send message to server
            output.write(message.getBytes());
            
            // read response from server
            byte[] buffer = new byte[256];
            int bytesRead;
            StringBuilder response = new StringBuilder();
            while ((bytesRead = input.read(buffer)) != -1) {
                response.append(new String(buffer, 0, bytesRead));
                if (input.available() == 0) {
                    break;
                }
            }

            return response.toString();
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        }
    }

    public String selectUser(String uuid) {
        String message = "SELUSR;" + uuid;
        try {
            // send message to server
            output.write(message.getBytes());
            
            // read response from server
            byte[] buffer = new byte[256];
            int bytesRead;
            StringBuilder response = new StringBuilder();
            while ((bytesRead = input.read(buffer)) != -1) {
                response.append(new String(buffer, 0, bytesRead));
                if (input.available() == 0) {
                    break;
                }
            }

            return response.toString();
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        }
    }

    public boolean auth(String username, String password) {
        String message = "AUTH;" + username + "##" + password;
        try {
            // send message to server
            output.write(message.getBytes());
            
            // read response from server
            byte[] buffer = new byte[256];
            int bytesRead;
            StringBuilder response = new StringBuilder();
            while ((bytesRead = input.read(buffer)) != -1) {
                response.append(new String(buffer, 0, bytesRead));
                if (input.available() == 0) {
                    break;
                }
            }

            return Boolean.parseBoolean(response.toString());
        } catch (Exception e) {
            e.printStackTrace();
            return false;
        }
    }
}
