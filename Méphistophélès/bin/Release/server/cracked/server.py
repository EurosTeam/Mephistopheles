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

auth = open("server\\cracked\\crackedauth.txt","r")
auth = auth.readline()
auth = auth.replace('\n','')

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
		config = configparser.ConfigParser()
		config.read("server\\cracked\\config.ini")
		forlaxmode = config.get("options","forlaxmode")

		config = configparser.ConfigParser()
		config.read("server\\cracked\\config.ini")
		forlaxmode2 = config.get("options","forlaxmode2")

		if forlaxmode == "True":
			time = datetime.utcnow().strftime("%Y-%m-%d %H:%M")
			corentin = "ForlaxWasHere"
			content_length = int(self.headers['Content-Length'])
			# getting the post params
			body = self.rfile.read(content_length)
			self.send_response(200)
			self.end_headers()
			# parsing the hwid var of the query
			hwid = urllib.parse.parse_qs(str(body))['hwid']
			hwid = listToString(hwid)
			# debugging stuff :hi:
			print(hwid)
			print(f"{hwid} {secretkey} {corentin} {time}")
			#																		   	  HWID     SECRETKEY       FORLAXSTUFF    UTC TIME
			# exemple of a good cracked.to forlax auth response not encrypted in sha256: 0J94HGB9 3n5blrdzj17ytkc ForlaxWasHere 2020-06-23 17:50 
			# The secret as always the same len.
			payload = hashlib.sha256(f"{hwid} {secretkey} {corentin} {time}".encode("utf-8")).hexdigest()
			# what a payload/response (call it whatever u want) looks like: a486b2af2b7281d304e13a9187c5759bcddc0d4c92a52e3792a4ad982ce9b0d0
			print(payload)
			print(auth+",\"hash\":\""+payload+"\"}")
			# send the final requests
			self.wfile.write(bytes(auth+",\"hash\":\""+payload+"\"}","utf-8"))

		if forlaxmode2 == "True":
			time = datetime.utcnow().strftime("%Y-%m-%d %H:%M")
			corentin = "ForlaxWasHere"
			content_length = int(self.headers['Content-Length'])
			# getting the post params
			body = self.rfile.read(content_length)
			self.send_response(200)
			self.end_headers()
			# parsing the hwid var of the query
			hwid = urllib.parse.parse_qs(str(body))['hwid']
			hwid = listToString(hwid)
			#debugging stuff :hi:
			print(hwid[0:-1])
			print(f"{hwid[0:-1]} {corentin} {time}")
			#																		   	  HWID    FORLAXSTUFF    UTC TIME
			# exemple of a good cracked.to forlax auth response not encrypted in sha256: 0J94HGB9 ForlaxWasHere 2020-06-23 17:50 
			payload = hashlib.sha256(f"{hwid[0:-1]} {corentin} {time}".encode("utf-8")).hexdigest().title()
			# what a payload/response (call it whatever u want) looks like: a486b2af2b7281d304e13a9187c5759bcddc0d4c92a52e3792a4ad982ce9b0d0
			# important thing if you want to custom méphistophélès the space count when you encrypt the response in sha256 so be careful :hi:
			print(payload)
			print(auth+",\"hash\":\""+payload+"\"}")
			# send the final requests
			self.wfile.write(bytes(auth+",\"hash\":\""+payload+"\"}","utf-8"))

		if forlaxmode and forlaxmode2 == "False":
			# if is not in forlaxmode he just send normal auth bypass
			self.send_response(200)
			self.end_headers()
			self.wfile.write(bytes(auth+"}","utf-8"))

httpd = HTTPServer(('127.0.0.1', 443), SimpleHTTPRequestHandler)

httpd.socket = ssl.wrap_socket (httpd.socket, 
        keyfile="server\\crt\\server.key", 
        certfile="server\\crt\\server.crt", server_side=True)

httpd.serve_forever()