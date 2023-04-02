import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.nio.ByteBuffer;
import java.nio.charset.StandardCharsets;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.Base64;

/**
 * Reference <a href="https://datatracker.ietf.org/doc/html/rfc6455#section-5.2">RFC6455</a>
 */
public class RFC6455 {

    public static void Handshake(InputStream in, OutputStream out) throws IOException {
        String key = null;
        byte[] buffer = new byte[1024];
        int bytes;

        // Read the client handshake request
        while ((bytes = in.read(buffer)) != -1) {
            String message = new String(buffer, 0, bytes);
            if (message.contains("Sec-WebSocket-Key: ")) {
                key = message.split("Sec-WebSocket-Key: ")[1].split("\r\n")[0];
                break;
            }
        }

        if (key != null) {
            // Generate the server's response
            String acceptKey = key + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
            byte[] sha1 = null;
            try {
                MessageDigest md = MessageDigest.getInstance("SHA-1");
                sha1 = md.digest(acceptKey.getBytes("UTF-8"));
            } catch (NoSuchAlgorithmException e) {
                e.printStackTrace();
            }
            String accept = Base64.getEncoder().encodeToString(sha1);
            String response = "HTTP/1.1 101 Switching Protocols\r\n"
                    + "Upgrade: websocket\r\n"
                    + "Connection: Upgrade\r\n"
                    + "Sec-WebSocket-Accept: " + accept + "\r\n\r\n";

            // Send the server's response
            out.write(response.getBytes());
            out.flush();
        }
    }

    public static byte[] encode(String data, boolean masked) {
        byte[] msg = data.getBytes(StandardCharsets.UTF_8);
        int opcode = 0x01;
        int messageLength = msg.length;

        byte[] frameHeader = generateFrameHeader(opcode, messageLength, masked);
        byte[] payload = masked ? maskData(msg, generateMaskingKey()) : msg;

        byte[] encodedMessage = new byte[frameHeader.length + payload.length];
        System.arraycopy(frameHeader, 0, encodedMessage, 0, frameHeader.length);
        System.arraycopy(payload, 0, encodedMessage, frameHeader.length, payload.length);

        return encodedMessage;
    }

    private static byte[] generateFrameHeader(int opcode, int payloadLength, boolean masked) {
        byte[] frameHeader;
    
        int maskFlag = masked ? 0x80 : 0x00;
        int payloadSize = payloadLength;
    
        if (payloadLength <= 125) {
            frameHeader = new byte[2];
            frameHeader[1] = (byte)(maskFlag | payloadSize);
        } else if (payloadLength <= 65535) {
            frameHeader = new byte[4];
            frameHeader[1] = (byte)(maskFlag | 126);
            frameHeader[2] = (byte)((payloadSize >> 8) & 0xFF);
            frameHeader[3] = (byte)(payloadSize & 0xFF);
        } else {
            frameHeader = new byte[10];
            frameHeader[1] = (byte)(maskFlag | 127);
            for (int i = 0; i < 8; i++) {
                frameHeader[i + 2] = (byte)((payloadSize >> ((7 - i) * 8)) & 0xFF);
            }
        }
    
        frameHeader[0] = (byte)(opcode | 0x80);
    
        return frameHeader;
    }
    

    private static byte[] generateMaskingKey() {
        byte[] maskingKey = new byte[4];
        new java.util.Random().nextBytes(maskingKey);
        return maskingKey;
    }

    private static byte[] maskData(byte[] payload, byte[] maskingKey) {
        byte[] maskedPayload = new byte[payload.length];
        for (int i = 0; i < maskedPayload.length; i++) {
            maskedPayload[i] = (byte) (payload[i] ^ maskingKey[i % 4]);
        }
        return maskedPayload;
    }

    public static byte[] decode(byte[] bytes) throws Exception {
        ByteBuffer buffer = ByteBuffer.allocate(bytes.length);
        for (int i = 0; i < bytes.length; i++ ) {
            buffer.put(bytes[i]);
        }
        buffer.flip();
        return decode(buffer);
    }

    private static byte[] decode(ByteBuffer buffer) throws Exception{
        if (buffer.remaining() < 2) {
            throw new Exception("Not enough dta to decode WebSocket message header");
        }

        //Read the first byte of the WebSocket message header
        byte b1 = buffer.get();
        boolean fin = (b1 & 0x80) != 0;
        int opcode = b1 & 0x0F;

        // Read the second byte of the WebSocket message header
        byte b2 = buffer.get();
        boolean mask = (b2 & 0x80) != 0;
        int payloadLength = b2 & 0x7F;

        // Decode the payload length
        if (payloadLength == 126) {
            if (buffer.remaining() < 2) {
                throw new Exception("Not enough data to decode WebSocket message header");
            }
            payloadLength = buffer.getShort() & 0xFFFF;
        } else if (payloadLength == 127) {
            if (buffer.remaining() < 8) {
                throw new Exception("Not enough data to decode WebSocket message header");
            }
            payloadLength = (int) buffer.getLong();
        }

        byte[] maskingKey = new byte[4];
        //Decode the masking key
        if (mask) {
            if (buffer.remaining() < 4) {
                throw new Exception("Not enough data to decode WebSocket message header");
            }
            buffer.get(maskingKey);
        }

        byte[] payloadData = new byte[payloadLength];
        //Decode the payload data
        if (payloadLength > 0) {
            if (buffer.remaining() < payloadLength) {
                throw new Exception("Not enough data to decode WebSocket message");
            }
            buffer.get(payloadData);
            if (mask) {
                //Apply the masking key to the payload data
                for (int i = 0; i < payloadData.length; i++) {
                    payloadData[i] ^= maskingKey[i % 4];
                }
            }
        }

        return payloadData;
    }
}
