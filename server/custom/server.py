from http.server import HTTPServer, BaseHTTPRequestHandler
import configparser
import ssl

auth = open("server\\custom\\custom.txt","r")
auth = auth.readline()
auth = auth.replace('\n','')

port = configparser.ConfigParser()
port.read("server\\custom\\config.ini")
port = port.get("options","port")
port = int(port)

class SimpleHTTPRequestHandler(BaseHTTPRequestHandler):

	def do_GET(self):
		self.send_response(200)
		self.end_headers()
		self.wfile.write(bytes(auth,"utf-8"))

	def do_POST(self):
		self.send_response(200)
		self.end_headers()
		self.wfile.write(bytes(auth,"utf-8"))



httpd = HTTPServer(('', port), SimpleHTTPRequestHandler)
httpd.socket = ssl.wrap_socket(httpd.socket,keyfile="crt\\server.key",certfile="crt\\server.crt",server_side=True)
httpd.serve_forever()