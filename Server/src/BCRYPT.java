import org.bouncycastle.crypto.generators.BCrypt;
import org.bouncycastle.util.encoders.Hex;

public class BCRYPT {
    public static int WORK_FACTOR = 12;
    public static int SALT_LENGTH = 16;

    public static String Hash(String password, byte[] salt) {
        byte[] hash = BCrypt.generate(password.getBytes(), salt, WORK_FACTOR);
        return new String(Hex.toHexString(hash));
    }

    public static boolean Check(String password, String hashedPassword, byte[] salt) {
        String hash = Hash(password, salt);
        return hashedPassword.equals(hash);
    }
}
