from http.server import HTTPServer, BaseHTTPRequestHandler
import ssl
import datetime
import hashlib
from urllib.parse import urlparse
import urllib
import time
import configparser

secret = configparser.ConfigParser()
secret.read("server\\nulled\\config.ini")
secretkey = secret.get("secret","secretkey")

print(secretkey)

def listToString(s):
	str1 = ""
	for ele in s:
		str1 += ele
	
	return str1

class SimpleHTTPRequestHandler(BaseHTTPRequestHandler):

	def do_GET(self):
		hwid = ""
		parsed = urlparse(self.path)
		print(parsed.query)
		self.send_response(200)
		self.end_headers()
		# getting hwid params of the query
		hwid = urllib.parse.parse_qs(parsed.query)["Hwid"]
		authnulled = urllib.parse.parse_qs(parsed.query)["Auth"]
		hwid = listToString(hwid)
		authnulled = listToString(authnulled)
		# debugging stuff
		print(str(round(time.time()/200)*200))
		print(hwid)
		# all the documentation of nulledauth can be found here: https://nulledauth.net/documentation.html
		payload = hashlib.sha256(f"{secretkey} {authnulled} {hwid} {round(time.time()/200)*200}".encode("utf-8")).hexdigest()
		print(payload)
		self.wfile.write(bytes(payload.upper(),"utf-8"))

httpd = HTTPServer(('127.0.0.1', 443), SimpleHTTPRequestHandler)

httpd.socket = ssl.wrap_socket(httpd.socket,server_side=True,certfile="server\\crt\\server.crt",keyfile="server\\crt\\server.key")

httpd.serve_forever()