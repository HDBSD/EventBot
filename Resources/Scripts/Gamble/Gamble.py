import os

x = 0
# Called when the script is loaded.
def Init():
    print("[PHY] " + str(os.getcwd()))
    return

# Called whenever a chat message is recieved
def Execute(data):
    print("[PHY] " + data.UserName + ": " + data.Message + " ---=--- " + str(data.IsChatMessage()))
    return

# Called every second without failure
def Tick():
    global x
    x = x + 1
    print("[PHY] Tick: " + str(x))
    return