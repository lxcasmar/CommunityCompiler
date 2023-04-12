import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;

public class SeatGeek {
    enum requests {
        GET
    }

    enum endpoints {
        events, 
        performers
    }

    private static final String BASE_URL = "https://api.seatgeek.com/2/";

    String get(String apiurl) {
        try {
            URL url = new URL(apiurl);
            HttpURLConnection con = (HttpURLConnection) url.openConnection();
            con.setRequestMethod(requests.GET.toString());

            int status = con.getResponseCode();
            System.out.println("Response code: "+ status);

            BufferedReader in = new BufferedReader(new InputStreamReader(con.getInputStream()));
            String inputLine;
            StringBuilder response = new StringBuilder();
            while ((inputLine = in.readLine()) != null) {
                response.append(inputLine);
            }
            in.close();
            return response.toString();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return "ERROR";
    }

    public static void main(String[] args) {
        java.security.Security.addProvider(new org.bouncycastle.jce.provider.BouncyCastleProvider());
        SeatGeek sg = new SeatGeek();
        String url = BASE_URL + 
                     endpoints.events.toString() +
                     "?client_id=" + ConfigUtils.getConfig("seat_geek_client_id")+
                     "&client_secret=" + ConfigUtils.getConfig("seat_geek_api_key");
        String response = sg.get(url);
        System.out.println(response);
    }
}