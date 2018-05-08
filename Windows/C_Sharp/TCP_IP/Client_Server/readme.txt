1/9/2013

Here is some example TCP/IP code I wrote Visual Studio with C#

See pdf for more information

There is both client and server code here.

The client creates a 146 byte message array. It opens a socket session
with the server waiting to connect.  The server receives messages
over the network, updates them and returns them to the client.
The client logs messages sent and received and keeps statistics.


