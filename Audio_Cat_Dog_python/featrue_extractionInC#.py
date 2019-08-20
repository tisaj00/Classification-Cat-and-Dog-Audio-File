import sys

import pandas as pd
import os
import librosa
import numpy as np


def extract_features(file_name):
    try:
        audio, sample_rate = librosa.load(file_name, res_type='kaiser_fast')
        mfccs = librosa.feature.mfcc(y=audio, sr=sample_rate, n_mfcc=40)
        mfccsscaled = np.mean(mfccs.T, axis=0)

    except Exception as e:
        print("Error encountered while parsing file: ")
        print(e)
        return None

    return mfccsscaled


features = []
curr_cwd = os.getcwd()
path_file = sys.argv[1]
class_label = "class_name"
data = extract_features(path_file)
data_12 = [data[0], data[1], data[2], data[3], data[4], data[5], data[6], data[7], data[8], data[9], data[10], data[11]]
features.append(data_12)

featuresdf = pd.DataFrame(features, columns=["Value1", "Value2", "Value3", "Value4", "Value5", "Value6",
                                             "Value7", "Value8", "Value9", "Value10", "Value11", "Value12"])

print(data_12)
