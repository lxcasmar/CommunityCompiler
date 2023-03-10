public class EventClient extends Client {
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

    public boolean createEvent(String owneruuid, String name, String description, String location, String startDate, String endDate, String capacity) {
        String message = "CRTEVT;" + owneruuid + Server.PARAM_DELIMITER
                                   + name + Server.PARAM_DELIMITER 
                                   + description + Server.PARAM_DELIMITER 
                                   + location + Server.PARAM_DELIMITER 
                                   + startDate;
        if (endDate != null) {
            message += Server.PARAM_DELIMITER + "A" + endDate;
        }
        if (capacity != null) {
            message += Server.PARAM_DELIMITER + "B" + capacity;
        }

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
        String message = "ALLEVT;";
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

    public String selectEvent(String uuid) {
        String message = "SELEVT;" + uuid;
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
}
