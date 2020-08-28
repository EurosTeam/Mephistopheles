from http.server import HTTPServer, BaseHTTPRequestHandler
import ssl
from io import BytesIO
import re
from datetime import datetime
import hashlib
import time
import urllib
from urllib.parse import urlparse
import configparser

auth = "{\"auth\":true,\"username\":\"ForlaxPy\",\"group\":\"100\""

secret = configparser.ConfigParser()
secret.read("server\\cracked\\config.ini")
secretkey = secret.get("secret","secretkey")

def listToString(s):
	str1 = ""
	for ele in s:
		str1 += ele
	return str1

class SimpleHTTPRequestHandler(BaseHTTPRequestHandler):

	def do_GET(self):
		# dont pay attention at this
		self.send_response(200)
		self.end_headers()
		self.wfile.write(bytes(auth+"}","utf-8"))

	def do_POST(self):
		global body
		forlaxmode = False
		forlaxmode2 = False
		corentin = "ForlaxWasHere"
		time = datetime.utcnow().strftime("%Y-%m-%d %H:%M")
		config = configparser.ConfigParser()
		content_length = int(self.headers['Content-Length'])
		# getting the post params
		body = self.rfile.read(content_length)
		print(str(body))
		self.send_response(200)
		self.end_headers()
		if("pid" in str(body)):
			forlaxmode = True

		if("hwid" in str(body)):
			forlaxmode2 = True

		if("pid" in str(body) and "hwid" in str(body)):
			forlaxmode = True
			forlaxmode2 = False
			
		print(str(forlaxmode))
		print(str(forlaxmode2))

		if forlaxmode == True:
			# parsing the hwid var of the query
			hwid = urllib.parse.parse_qs(str(body))['hwid']
			hwid = listToString(hwid)
			#																		   	  HWID     SECRETKEY       FORLAXSTUFF    UTC TIME
			# exemple of a good cracked.to forlax auth response not encrypted in sha256: 0J94HGB9 3n5blrdzj17ytkc ForlaxWasHere 2020-06-23 17:50
			# The secret as always the same len.
			payload = hashlib.sha256(f"{hwid} {secretkey} {corentin} {time}".encode("utf-8")).hexdigest()
			# what a response looks like: a486b2af2b7281d304e13a9187c5759bcddc0d4c92a52e3792a4ad982ce9b0d0
			print(payload)
			print(auth+",\"hash\":\""+payload+"\"}")
			# send the final requests
			self.wfile.write(bytes(auth+",\"hash\":\""+payload+"\"}","utf-8"))

		if forlaxmode2 == True:
			# parsing the hwid var of the query
			hwid = urllib.parse.parse_qs(str(body))['hwid']
			hwid = listToString(hwid)
			#																		   	  HWID    FORLAXSTUFF    UTC TIME
			# exemple of a good cracked.to forlax auth response not encrypted in sha256: 0J94HGB9 ForlaxWasHere 2020-06-23 17:50
			payload = hashlib.sha256(f"{hwid[0:-1]} {corentin} {time}".encode("utf-8")).hexdigest().title()
			# what a response looks like: a486b2af2b7281d304e13a9187c5759bcddc0d4c92a52e3792a4ad982ce9b0d0
			#debugging stuff
			print(auth+",\"hash\":\""+payload+"\"}")
			# send the final requests
			self.wfile.write(bytes(auth+",\"hash\":\""+payload+"\"}","utf-8"))

httpd = HTTPServer(('127.0.0.1', 443), SimpleHTTPRequestHandler)

httpd.socket = ssl.wrap_socket (httpd.socket, 
        keyfile="server\\crt\\server.key", 
        certfile="server\\crt\\server.crt", server_side=True)

httpd.serve_forever()