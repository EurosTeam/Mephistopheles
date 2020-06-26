from http.server import HTTPServer, BaseHTTPRequestHandler
import configparser
import ssl

auth = configparser.ConfigParser()
auth.read("server\\custom\\config.ini")
authresponse = auth.get("response","response")

port = configparser.ConfigParser()
port.read("server\\custom\\config.ini")
port = port.get("options","port")
port = int(port)

class SimpleHTTPRequestHandler(BaseHTTPRequestHandler):

	def do_GET(self):
		self.send_response(200)
		self.end_headers()
		self.wfile.write(bytes(authresponse,"utf-8"))

	def do_POST(self):
		self.send_response(200)
		self.end_headers()
		self.wfile.write(bytes(authresponse,"utf-8"))



httpd = HTTPServer(('127.0.0.1', port), SimpleHTTPRequestHandler)
if port == 443:
	httpd.socket = ssl.wrap_socket(httpd.socket,keyfile="server\\crt\\server.key",certfile="server\\crt\\server.crt",server_side=True)
httpd.serve_forever()