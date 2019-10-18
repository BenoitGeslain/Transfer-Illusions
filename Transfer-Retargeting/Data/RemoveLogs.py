#! python3
import os

baseDir = os.getcwd() + "\\"
files = ["config.csv", "continous.csv", "discrete.csv"]

for f in files:
    try:
        os.remove(baseDir + f)
    except OSError as e:
        print(e)