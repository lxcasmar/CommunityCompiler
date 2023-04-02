public class ByteUtils {
    public static byte[] concat(byte[] a, byte[] b) {
        byte[] c = new byte[a.length + b.length];
        System.arraycopy(a, 0, c, 0, a.length);
        System.arraycopy(b, 0, c, a.length, b.length);
        return c;
    }

    public static byte[] concat(byte[] a, byte[] b, byte[] c) {
        byte[] d = concat(a,b);
        return concat(d,c);
    }

    public static byte[] subarray(byte[] source, int start, int end) {
        if (start < 0 || end > source.length || start > end) {
            throw new IllegalArgumentException("Invalid start or end index");
        }
        byte[] dest = new byte[end - start];
        System.arraycopy(source, start, dest, 0, end - start);
        return dest;
    }
}