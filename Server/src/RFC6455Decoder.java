import java.nio.ByteBuffer;

/**
 * Reference <a href="https://datatracker.ietf.org/doc/html/rfc6455#section-5.2">RFC6455</a>
 */
public class RFC6455Decoder {
    private boolean fin;
    private int opcode;
    private boolean mask;
    private int payloadLength;
    private byte[] maskingKey;
    private byte[] payloadData;

    public RFC6455Decoder() {
        fin = false;
        opcode = -1;
        mask = false;
        payloadLength = -1;
        maskingKey = new byte[4];
        payloadData = null;
    }

    public void decode(byte[] bytes) throws Exception {
        ByteBuffer buffer = ByteBuffer.allocate(bytes.length);
        for (int i = 0; i < bytes.length; i++ ) {
            buffer.put(bytes[i]);
        }
        buffer.flip();
        decode(buffer);
    }

    private void decode(ByteBuffer buffer) throws Exception{
        if (buffer.remaining() < 2) {
            throw new Exception("Not enough dta to decode WebSocket message header");
        }

        //Read the first byte of the WebSocket message header
        byte b1 = buffer.get();
        fin = (b1 & 0x80) != 0;
        opcode = b1 & 0x0F;

        // Read the second byte of the WebSocket message header
        byte b2 = buffer.get();
        mask = (b2 & 0x80) != 0;
        payloadLength = b2 & 0x7F;

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

        //Decode the masking key
        if (mask) {
            if (buffer.remaining() < 4) {
                throw new Exception("Not enough data to decode WebSocket message header");
            }
            buffer.get(maskingKey);
        }

        //Decode the payload data
        if (payloadLength > 0) {
            if (buffer.remaining() < payloadLength) {
                throw new Exception("Not enough data to decode WebSocket message");
            }
            payloadData = new byte[payloadLength];
            buffer.get(payloadData);
            if (mask) {
                //Apply the masking key to the payload data
                for (int i = 0; i < payloadData.length; i++) {
                    payloadData[i] ^= maskingKey[i % 4];
                }
            }
        }
    }

    public byte[] getPayloadData() {
        return payloadData;
    }
}
