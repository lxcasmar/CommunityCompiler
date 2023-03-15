import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.nio.file.Files;
import java.util.HashMap;
import java.util.Scanner;

import javax.crypto.SecretKey;

public class ConfigUtils {
    private static String CONFIG_FILE = "config.txt";
    private static HashMap<String, String> configs;

    // keeping the configs in memory is a bad idea.... should delete this
    public static void parseAll() {
        configs = new HashMap<String, String>();
        try {
            Scanner scanner = new Scanner(new File(CONFIG_FILE));
            while (scanner.hasNextLine()) {
                String line = scanner.nextLine();
                String key = line.split("=")[0];
                String value = line.split("=")[1];
                configs.put(key, value);
            }
            scanner.close();
        } catch (FileNotFoundException e) {
            e.printStackTrace();
        }
    }

    public static String getConfig(String key) {
        return configs.get(key);
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
        // encrypt the config file
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
        
        SecretKey configKey = KeyStoreGenerator.recoverKey();
        if (mode == 'e') {
            // encrypt
            try {
                byte[] contents = Files.readAllBytes(new File(CONFIG_FILE).toPath());
                byte[][] encrypted = AES.encrypt(contents, configKey);
                FileOutputStream fos = new FileOutputStream(CONFIG_FILE);
                fos.write(ByteUtils.concat(encrypted[1], encrypted[0]));
                fos.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
        } else if (mode == 'd') {
            // decrypt
            try {
                byte[] contents = Files.readAllBytes(new File(CONFIG_FILE).toPath());
                byte[] iv = ByteUtils.subarray(contents, 0, 16);
                byte[] ciphertext = ByteUtils.subarray(contents, 16, contents.length);
                byte[] plaintext = AES.decrypt(ciphertext, iv, configKey);
                FileOutputStream fos = new FileOutputStream(CONFIG_FILE);
                fos.write(plaintext);
                fos.close();
            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }
}
