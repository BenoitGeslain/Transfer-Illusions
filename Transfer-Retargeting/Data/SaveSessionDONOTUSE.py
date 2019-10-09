#! python3
import os
import shutil

baseDir = os.getcwd() + "\\"
files = ["config.csv", "continous.csv", "discrete.csv"]

print()
print("Input folder name")
path = input()
print()

try:
    os.mkdir(path)
except OSError as e:
    print("Copy failed trying to copy data in " + path + ".failed")
    try:
        os.mkdir(path + ".failed")
        try:
            for f in files:
                shutil.copyfile(baseDir + f, baseDir + path + ".failed" + "\\" + f)
            print()
            print("Copy successful")
        except OSError as e:
            print()
            print("COPY HAS FAILED")
            print(e)
    except OSError:
        print()
        print("COPY HAS FAILED")
        print(e)
else:
    try:
        for f in files:
            shutil.copyfile(baseDir + f, baseDir + path + "\\" + f)
        print()
        print("Copy successful")
    except OSError as e:
        print()
        print("COPY HAS FAILED")
        print(e)