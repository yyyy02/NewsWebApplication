import pymssql
import copy
import random
import heapq
import sys
import numpy as np
import pprint as pp
import pandas as pd 
from matplotlib import pyplot as plt
from mpl_toolkits.mplot3d import Axes3D # 空间三维画图
from sklearn.feature_extraction import DictVectorizer

df = pd.read_csv("D:/Desktop/alg/try.csv")
data_list = df.values.ravel().tolist()
print(data_list)