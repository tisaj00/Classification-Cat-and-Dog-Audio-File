import pandas as pd
import os
import librosa
import numpy as np
import csv


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
combo_dir = os.path.join("cats_dogs")
content = {}
for root, dirs, files in os.walk(combo_dir):
    for subdir in dirs:
        content[os.path.join(root, subdir)] = []
    content[root] = files

for dirs in content:
    for file_name in content[dirs]:

        path_file = os.path.join(dirs, file_name)
        prediction = 0
        if not path_file.endswith(".wav"):
            continue
        if path_file.startswith("cats_dogs\cat_"):
            prediction = 1
        else:
            prediction = 0
        print(path_file)

        class_label = "class_name"
        data = extract_features(path_file)
        data_12 = [file_name, data[0], data[1], data[2], data[3], data[4], data[5], data[6], data[7], data[8], data[9], \
                   data[10], data[11], prediction]
        features.append(data_12)

        with open('dog_cat.csv', 'w', newline='') as f:
            writer = csv.DictWriter(f, fieldnames=["Name", "Value1", "Value2", "Value3", "Value4", "Value5", "Value6",
                                                   "Value7", "Value8", "Value9", "Value10", "Value11", "Value12",
                                                   "Prediction"])
            writer.writeheader()
            writer = csv.writer(f)
            writer.writerows(zip(features))
            text = open("dog_cat.csv", "r")
            text = ''.join([i for i in text]) \
                .replace("[", " ").replace("]", " ").replace("'", " ").replace(",", " ,")
            x = open("dog_cat1.csv", "w")
            x.writelines(text)

featuresdf = pd.DataFrame(features, columns=["Name", "Value1", "Value2", "Value3", "Value4", "Value5", "Value6",
                                             "Value7", "Value8", "Value9", "Value10", "Value11", "Value12", "Prediction"
                                             ])

print('Finished feature extraction from ', len(featuresdf), ' files')
