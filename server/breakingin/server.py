from http.server import HTTPServer, BaseHTTPRequestHandler
import ssl

#simplest auth to bypass :kek:

class SimpleHTTPRequestHandler(BaseHTTPRequestHandler):

	def do_GET(self):
		self.send_response(200)
		self.end_headers()
		self.wfile.write(b"{\"status\":true,\"data\":{\"message\":\"success\"},\"hash\":\"null\"}")

	def do_POST(self):
		self.send_response(200)
		self.end_headers()
		self.wfile.write(b"{\"status\":true,\"data\":{\"message\":\"success\"},\"hash\":\"null\"}")

httpd = HTTPServer(('127.0.0.1',443),SimpleHTTPRequestHandler)

httpd.socket = ssl.wrap_socket(httpd.socket,keyfile="server\\crt\\server.key",certfile="server\\crt\\server.crt")

httpd.serve_forever()