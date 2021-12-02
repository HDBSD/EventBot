x = 0


# Called when the script is loaded.
def Init():
    print("test3")
    return

# Called whenever a chat message is recieved
def Execute(data):
    print("script2")
    return

# Called every second without failure
def Tick():
    global x
    x = x + 2
    print(x)
    return