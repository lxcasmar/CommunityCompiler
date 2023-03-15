import java.security.GeneralSecurityException;
import java.security.NoSuchAlgorithmException;
import java.security.NoSuchProviderException;
import javax.crypto.Cipher;
import javax.crypto.KeyGenerator;
import javax.crypto.SecretKey;
import javax.crypto.spec.IvParameterSpec;
import org.bouncycastle.util.encoders.Hex;

public class AES {
    private static final String PROVIDER = "BC";
    public static final String ALGORITHM = "AES/CBC/PKCS7Padding";

    /**
     * Generates a new AES key
     * @param keySize 128, 192, or 256 bits
     * @return a new AES key
     * @throws GeneralSecurityException
     */
    public static SecretKey generateKey (int keySize) {
        KeyGenerator keyGen;
        try {
            keyGen = KeyGenerator.getInstance("AES", PROVIDER);
            keyGen.init(keySize);
            return keyGen.generateKey();
        } catch (NoSuchAlgorithmException e) {
            e.printStackTrace();
        } catch (NoSuchProviderException e) {
            e.printStackTrace();
        }
        return null;
    }

    /**
     * Encrypts an input string
     * @param pt  The plaintext
     * @param key The AES key
     * @return {ciphertext, iv} 
     */
    public static byte[][] encrypt(byte[] pt, SecretKey key) {
        try {
            Cipher aes = Cipher.getInstance(ALGORITHM, PROVIDER);
            aes.init(Cipher.ENCRYPT_MODE, key);

            byte[] ct = aes.doFinal(pt);
            byte[] iv = aes.getIV();
            
            return new byte[][] {ct, iv};
        } catch (GeneralSecurityException e) {
            e.printStackTrace();
        }
        return null;
    }

    public static byte[][] encrypt(String pt, SecretKey key) {
        return encrypt(pt.getBytes(), key);
    }

    /**
     * Decrypts an input string
     * @param ct  The ciphertext
     * @param key The AES key
     * @param iv  The initialization vector
     * @return The plaintext as a <c>byte[]</c>
     */
    public static byte[] decrypt(byte[] ct, byte[] iv, SecretKey key) {
        try {
            Cipher aes = Cipher.getInstance(ALGORITHM, PROVIDER);
            aes.init(Cipher.DECRYPT_MODE, key, new IvParameterSpec(iv));
            return aes.doFinal(ct);
        } catch (GeneralSecurityException e) {
            System.out.println("Error decrypting: " + Hex.toHexString(ct));
            e.printStackTrace();
        }
        return null;
    }
}
