# Usage

## Compile

In directory `Server` (the root directory for this Java project)

```
$ javac -d bin -cp bin:lib/* src/*.java
```

## Run

1. Open 3 terminal windows 
2. Terminal 1 - User server.
```
$ java -cp bin:lib/* RunUserServer [<port>] [<name>]
```
Note, if you want to specify host, then must also specify port for now... This should probably be changed for a more elegant way to handle arguments
3. Terminal 2 - Event server.
```
$ java -cp bin:lib/* RunEventServer [<port>] [<name>]
```
At this point, both servers are running and the databases should be functioning. To test the functionalities, there is also corresponding clients for each of the servers. Note that the following will not be used in the final project, as the phone application will act as the client.

4. Terminal 3 - Clients
```
$ java -cp bin:lib/* App <UserServerPort> [<UserServerHost>] <EventServerPort> [<EvenServerHost>]
```
No need to specify either host if running locally. Must specify both hosts otherwise.
All user I/O happens in Terminal 3. Enter the `HELP` command for information on the supported commands.  

# Security Stuff

## Config file
`config/` will be used to store encrypted files containing API keys and other secrets. As part of the server startup, we need to encrypt all of them. To do so, run the following:
```
Server$ java -cp bin:lib/* ConfigUtils -e
```
Whenever adding a new secret, it is important to decrypt the rest of the keys fist, as encrypting a file multiple times can cause issues  
To decrypt the files:
```
Server$ java -cp bin:lib/* ConfigUtils -d
```

## KeyStore
As mentioned above, we need an AES cryptographic key to encrypt and decrypt the config files. We need this key to persist beyond memory at runtime, so it needs to be stored on disk. We use Java KeyStores to achieve this.  

Since there is currently no need for more than a single key, the following creates the keystore file and also stores the AES key in that file
```
Server$ java -cp bin:lib/* KeyStoreGenerator
```
Make sure to generate the keystore file BEFORE attempting to encrypt the config files. A good way to check if the file is encrypted is to open the files with *nano*. If there are a bunch of `?` then it's encrypted.
The file is password protected, and each key within the file is also protected.