import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.nio.file.Files;

import javax.crypto.SecretKey;

public class ConfigUtils {
    public static String getConfig(String key) {
        String path = "config/" + key + ".enc";
        SecretKey configKey = KeyStoreGenerator.recoverKey(key);
        try {
            byte[] contents = Files.readAllBytes(new File(path).toPath());
            byte[] iv = ByteUtils.subarray(contents, 0, 16);
            byte[] ciphertext = ByteUtils.subarray(contents, 16, contents.length);
            byte[] plaintext = AES.decrypt(ciphertext, iv, configKey);
            return new String(plaintext).trim();
        } catch (IOException e) {
            e.printStackTrace();   
        }
        return null;
    }

    public static void encryptConfigs() {
        // reload AES key from keystore
        SecretKey configKey;
        // encrypt the e config file
        try {
            File configDirectory = new File("config");
            File[] configFiles = configDirectory.listFiles();
            for (File f : configFiles) {
                if (!f.getName().substring(f.getName().length() -4).equals(".enc")) {
                    continue;
                }
                //System.out.println("cur enc file name: " + f.getName().substring(0, f.getName().length() - 4));
                configKey = KeyStoreGenerator.recoverKey(f.getName().substring(0, f.getName().length() - 4));
                byte[] contents = Files.readAllBytes(f.toPath());
                byte[][] encrypted = AES.encrypt(contents, configKey);
                FileOutputStream fos = new FileOutputStream(f);
                fos.write(ByteUtils.concat(encrypted[1], encrypted[0]));
                fos.close();
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private static void decryptConfigs() {
        SecretKey configKey;
        try {
            File configDirectory = new File("config");
            File[] configFiles = configDirectory.listFiles();
            for (File f : configFiles) {
                if (!f.getName().substring(f.getName().length() -4).equals(".enc")) {
                    continue;
                }
                //System.out.println("cur enc file name: " + f.getName().substring(0, f.getName().length() - 4));
                configKey = KeyStoreGenerator.recoverKey(f.getName().substring(0, f.getName().length() - 4));
                byte[] contents = Files.readAllBytes(f.toPath());
                byte[] iv = ByteUtils.subarray(contents, 0, 16);
                byte[] ciphertext = ByteUtils.subarray(contents, 16, contents.length);
                byte[] plaintext = AES.decrypt(ciphertext, iv, configKey);
                FileOutputStream fos = new FileOutputStream(f);
                fos.write(plaintext);
                fos.close();
            }
        } catch (IOException e) {
            e.printStackTrace();   
        }
    }

    /**
     * encrypts the config file.
     * Note that if the config file has to be modified, run this in decrypt mode FIRST. 
     * Then, modify the file and run this in encrypt mode.
     * @param args
     */
    public static void main (String [] args) {
        java.security.Security.addProvider(new org.bouncycastle.jce.provider.BouncyCastleProvider());
        // reload AES key from keystore
        // encrypt the e config file
        char mode;
        try {
            mode = args[0].charAt(1);
            if (mode != 'e' && mode != 'd') {
                System.err.println("Usage: java ConfigUtils <-e|-d>");
            }
        } catch (ArrayIndexOutOfBoundsException e) {
            System.err.println("Usage: java ConfigUtils <-e|-d>");
            return;
        }
        
        if (mode == 'e') {
            encryptConfigs();
        } else if (mode == 'd') {
            decryptConfigs();
        }
    }
}
