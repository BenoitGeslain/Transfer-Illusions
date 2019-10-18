#! python3
import os
import shutil

path = "Participants"
baseDir = os.getcwd() + "\\"
files = ["config.csv", "continous.csv", "discrete.csv"]

print()
print("Input participant name :")
participant = input()
print()

try:
    os.mkdir(path)
except OSError:
    pass

if not os.path.exists(baseDir + path + "\\" + participant + "_" + files[0]):
    for f in files:
        shutil.copyfile(baseDir + f, baseDir + path + "\\" + participant + "_" + f)
    print()
    print("Copy successful")
else:
    print()
    print("File already exists, trying to copy as " + participant + "_" + files[0] + ".failed")
    if not os.path.exists(baseDir + path + "\\" + participant + "_" + files[0] + ".failed"):
        for f in files:
            shutil.copyfile(baseDir + f, baseDir + path + "\\" + participant + "_" + f + ".failed")
        print()
        print("Copy successful")
    else:
        print()
        print("COPY HAS FAILED")