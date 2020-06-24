from http.server import HTTPServer, BaseHTTPRequestHandler
import ssl
import datetime
import hashlib
from urllib.parse import urlparse
import urllib
import time
import configparser

#get the secret key
secret = open("server\\nulled\\secret.txt","r")
secret = secret.readline()
secret = secret.replace('\n','')

print(secret)

def listToString(s):
	str1 = ""
	for ele in s:
		str1 += ele
	
	return str1

class SimpleHTTPRequestHandler(BaseHTTPRequestHandler):

	def do_GET(self):
		#nulledauth.net auth bypass
		config = configparser.ConfigParser()
		config.read("server\\nulled\\config.ini")
		nulledauthmode = config.get("options","nulledauthmode")
		#get the value of nulledauthmode of the config.ini file
		if nulledauthmode == "True":
			hwid = ""
			self.send_response(200)
			self.end_headers()
			parsed = urlparse(self.path)
			print(parsed.query)
			#getting hwid params of the query
			hwid = urllib.parse.parse_qs(parsed.query)["Hwid"]
			authnulled = "cain"
			hwid = listToString(hwid)
			#debugging stuff
			print(str(round(time.time()/200)*200))
			print(hwid)
			#all the documentation of nulledauth can be found here: https://nulledauth.net/documentation.html
			payload = hashlib.sha256(f"{secret} {authnulled} {hwid} {round(time.time()/200)*200}".encode("utf-8")).hexdigest()
			print(payload)
			self.wfile.write(bytes(payload.upper(),"utf-8"))

		if nulledauthmode == "False":
			#if the nulledauthmode false send a simple bypass requests for nulled.to auth
			self.send_response(200)
			self.end_headers()
			self.wfile.write(b"{\"auth\":true,\"status\":\"success\"}")

httpd = HTTPServer(('', 443), SimpleHTTPRequestHandler)

httpd.socket = ssl.wrap_socket(httpd.socket,server_side=True,certfile="crt\\server.crt",keyfile="crt\\server.key")

httpd.serve_forever()