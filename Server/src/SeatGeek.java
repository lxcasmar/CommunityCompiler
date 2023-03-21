// import okhttp3.*;

// public class SeatGeek {
//     private static final String BASE_URL = "https://api.seatgeek.com/2/";
//     private final OkHttpClient client = new OkHttpClient();

//     public enum endpoints {
//         events,
//         performers
//     }
//     public String get(endpoints endpoint) {
//         String url = BASE_URL
//                    + endpoint.toString()
//                    + "?client_id=" + ConfigUtils.getConfig("seat_geek_client_id")
//                    + "&client_secret=" + ConfigUtils.getConfig("seat_geek_api_key");
//         Request request = new Request.Builder()
//             .url(url)
//             .build();
//         try (Response response = client.newCall(request).execute()) {
//             return response.body().string();
//         } catch (Exception e) {
//             e.printStackTrace();
//         }
//         return null;
//     }
// }