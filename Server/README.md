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
All user I/O happens in Terminal 3. Enter the `HELP` command for information on the supported commands
