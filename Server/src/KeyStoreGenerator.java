import javax.crypto.SecretKey;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.security.KeyStore;
import java.util.Scanner;

public class KeyStoreGenerator {
    private static final String KEYSTORE_TYPE = "JCEKS";
    private static final String KEYSTORE_FILENAME = "keystore.jceks";
    private static Scanner sc = new Scanner(System.in);

    public static void generateKeyStore() {
        if (new File(KEYSTORE_FILENAME).exists()) {
            System.err.println("Keystore already exists. Delete it first if you'd like to regenerate it.");
            return;
        }
        
        System.out.println("Create keystore password: ");
        char[] KEYSTORE_PASSWORD = sc.nextLine().toCharArray();
        try {
            KeyStore keystore = KeyStore.getInstance(KEYSTORE_TYPE);
            keystore.load(null, KEYSTORE_PASSWORD);

            FileOutputStream fos = new FileOutputStream(KEYSTORE_FILENAME);
            keystore.store(fos, KEYSTORE_PASSWORD);
            fos.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    public static SecretKey recoverKey(String alias) {
        // can modify this if need to store/recover more than one key
        System.out.println("Enter keystore password: ");
        char[] KEYSTORE_PASSWORD = sc.nextLine().toCharArray();
        System.out.println("Enter key password for " + alias + ": ");
        char[] KEY_PASSWORD = sc.nextLine().toCharArray();
        try {
            KeyStore keystore = KeyStore.getInstance(KEYSTORE_TYPE);
            FileInputStream fis = new FileInputStream(KEYSTORE_FILENAME);
            keystore.load(fis, KEYSTORE_PASSWORD);

            KeyStore.ProtectionParameter protParam = new KeyStore.PasswordProtection(KEY_PASSWORD);
            KeyStore.SecretKeyEntry keyEntry = (KeyStore.SecretKeyEntry) keystore.getEntry(alias, protParam);
            SecretKey configKey = keyEntry.getSecretKey();
            fis.close();
            return configKey;
        } catch (Exception e) {
            e.printStackTrace();
        }
        return null;
    }

    public static void addEntry(String alias, SecretKey key) {
        System.out.println("Enter keystore password: ");
        char[] KEYSTORE_PASSWORD = sc.nextLine().toCharArray();
        try {
            System.out.println("Create a key password for " + alias + ": ");
            char[] KEY_PASSWORD = sc.nextLine().toCharArray();
            KeyStore keystore = KeyStore.getInstance(KEYSTORE_TYPE);
            FileInputStream fis = new FileInputStream(KEYSTORE_FILENAME);
            keystore.load(fis, KEYSTORE_PASSWORD);

            KeyStore.SecretKeyEntry keyEntry = new KeyStore.SecretKeyEntry(key);
            KeyStore.ProtectionParameter protParam = new KeyStore.PasswordProtection(KEY_PASSWORD);
            keystore.setEntry(alias, keyEntry, protParam);

            FileOutputStream fos = new FileOutputStream(KEYSTORE_FILENAME);
            keystore.store(fos, KEYSTORE_PASSWORD);
            fos.close();
            fis.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    public static void main(String[] args) {
        java.security.Security.addProvider(new org.bouncycastle.jce.provider.BouncyCastleProvider());
        generateKeyStore();
        addEntry("seat_geek_api_key",AES.generateKey(256));
        addEntry("seat_geek_client_id", AES.generateKey(256));
        sc.close();
    }
}
