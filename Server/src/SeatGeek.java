import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;

import org.json.JSONArray;
import org.json.JSONObject;

public class SeatGeek {
    enum requests {
        GET
    }

    enum endpoints {
        events, 
        performers,
        venues
    }

    private static final String BASE_URL = "https://api.seatgeek.com/2/";

    JSONObject get(String apiurl) {
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

            JSONObject jsonResponse = new JSONObject(response.toString());
            //return jsonResponse.toString(4);
            return jsonResponse;
        } catch (Exception e) {
            e.printStackTrace();
        }
        return null;
    }

    // TODO: need to create a user with UUID 0 for API
    void populateEvents() {
        String url = BASE_URL + 
                     endpoints.events.toString() +
                     "?client_id=" + ConfigUtils.getConfig("seat_geek_client_id")+
                     "&client_secret=" + ConfigUtils.getConfig("seat_geek_api_key");
        JSONObject response = get(url);
        JSONArray eventsArray = response.getJSONArray("events");
        System.out.println("Number of events: " + eventsArray.length());
        for (int i = 0; i < Math.min(eventsArray.length(),50); i++) {
            JSONObject event = eventsArray.getJSONObject(i);
            String title = event.getString("title");
            JSONObject venue = event.getJSONObject("venue");
            String location = venue.getString("extended_address") + ", " + venue.getString("display_location");
            String eventUrl = event.getString("url");
            String description = event.getString("description");
            String type = event.getString("type");
            JSONObject performer = event.getJSONArray("performers").getJSONObject(0);
            String image = performer.getString("image");
            String datetime = event.getString("datetime_utc");
            String endDatetime = null;
            if (!event.isNull("enddatetime_utc")) {
                endDatetime = event.getString("enddatetime_utc");
            }
            String owner = "da1b490b-1225-4e25-9627-eeb0e5d792cd";
            EventThread.seatGeekCrtEvt(new String [] {title, location, eventUrl, description, type, image, datetime, endDatetime, owner});
        }
    }

    public static void main(String[] args) {
        java.security.Security.addProvider(new org.bouncycastle.jce.provider.BouncyCastleProvider());
        SeatGeek sg = new SeatGeek();
        sg.populateEvents();
    }
}