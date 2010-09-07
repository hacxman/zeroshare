import Zeroconf as Z
import sys
import socket
import hashlib
import random
import asyncore
import stat, os
import time

#r = Z.Zeroconf('192.168.69.19')
r = Z.Zeroconf('0.0.0.0')

usage = """Zeroshare usage:
To send a file: zeroshare.py -s filename
To receive a file: zeroshare.py -r"""

CHAL_HASH = ''
CHAL_PIN  = ''
PIN = ''
FNAME = ''

class MyListener(object):
    def removeService(self, server, type, name):
        print "Service", repr(name), "removed"
    
    def addService(self, server, _type, name):
#        print "Service", repr(name), "added"
        info = server.getServiceInfo(_type, name)
        h = info.getProperties()['r']
        hs = hashlib.sha1(h + PIN).hexdigest()
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        s.connect((socket.inet_ntoa(info.getAddress()), info.getPort()))
        s.send(hs)
        data = s.recv(1024)
        data = data.splitlines()
#        data = s.readline()
        print repr(data)        
        fsize = int(data[1])
        with open(data[0]+'cpy', 'w+') as fout:
            rcvd = 0            
            while True:
                ltime = time.time()
                data = s.recv(1024)                
                rcvd += len(data)                
                if not data: break
                fout.write(data)
                ntime = time.time()
                #print rcvd, 'of', fsize, '%.2f' % (100*float(rcvd)/fsize),'% ', 1024.0/(ntime-ltime),'Bps    \r',
        s.close()
        print '\nFILE RCVD'       
        
#        print 'Additional info:', info
#        print socket.inet_ntoa(info.getAddress())
#        print info.getPort()
#        print info.getProperties()
        

class Connection_listener(asyncore.dispatcher):
    def __init__(self):
        asyncore.dispatcher.__init__(self)
        self.create_socket(socket.AF_INET, socket.SOCK_STREAM)
        self.set_reuse_addr()
        self.bind(('', 55555))
        self.listen(1)
        self.buffer = ''
    
#    def handle_connect(self):
#        print "Handle connect"
#        self.send('hello')
#        self.close()
    
#    def handle_read(self):
#        print self.recv(8192)
        
    def handle_accept(self):
        conn, addr = self.accept()
        print "Handle_accept" ,conn, addr
        resp = conn.recv(40)
        if resp == hashlib.sha1(CHAL_HASH + CHAL_PIN).hexdigest():
            print "YIPIKAYE"
            #conn.send("%d\n" % len(FNAME) )
            bsend = 0
            bsend += conn.send("%s\n" % FNAME )
            bsend += conn.send("%d\n" % os.stat(FNAME)[stat.ST_SIZE])
            bsend += conn.send('\0' * (1024-bsend))
            print bsend
            with open(FNAME) as fin:
                while True:
                    data = fin.read(1024)
                    if data == '':
                        break
                    conn.send(data)
            print 'FILE SENT'
        else:
            print "FFFFuuu"
            conn.close()
        conn.close()

#    def writable(self):
#        return (len(self.buffer) > 0)
    
#    def handle_write(self):
#        sent = self.send(self.buffer)
#        self.buffer = self.buffer[sent:]
    
#    def handle_close(self):
#        self.close()

def receive():
    pass
    

def send():
    local_ip = socket.gethostbyname(socket.gethostname())
    local_ip = '192.168.69.19'
    print 'My IP is', local_ip
    local_ip = socket.inet_aton(local_ip)
    
    global CHAL_HASH, CHAL_PIN
    CHAL_HASH = hashlib.sha1(str(random.randint(0,2**64-1))).hexdigest()
    CHAL_PIN = "%d" % random.randint(0,9999)
    CHAL_PIN.zfill(4)
    print "PIN:", CHAL_PIN
    
    svc1 = Z.ServiceInfo('_zeroshare._tcp.local.',
                            '%s._zeroshare._tcp.local.' % sys.argv[2],
                            address = local_ip,
                            port = 55555,
                            weight = 0, priority = 0,
                            properties = {'description': 'Zeroshare file sharing server', 
                                          'r': CHAL_HASH
                                         })
    r.registerService(svc1)
    
    srv = Connection_listener()
    asyncore.loop()
    
#    while(1): pass

if __name__ == "__main__":
    if len(sys.argv) == 1:
        print usage
        exit(1)
    
#    browser.run()
    
    if sys.argv[1] == '-r':        
        print 'ENTER PIN:',
        PIN = sys.stdin.readline().strip()
        listener = MyListener()
        browser = Z.ServiceBrowser(r, "_zeroshare._tcp.local.", listener)        
        receive()
    elif sys.argv[1] == '-s':
        if len(sys.argv) != 3:
            print 'buuuu'
            exit(1)
        FNAME = sys.argv[2]
        send()
    
