import os
import tensorflow as tf
import numpy as np
from PIL import Image
from sklearn.model_selection import train_test_split
from sklearn.utils import shuffle

# Create Lists
X = []
Y = []

str_to_num = {"gnome": 0, "drone": 1}

gnome_folder = "data/gnomeImages"
drone_folder = "data/droneImages"

imageHeight = 200
imageWidth = 200


# Load Images
def create_data(folder, name):
    for i in os.listdir(folder):
        if i == ".DS_Store":
            continue

        print("Current File: {}".format(os.path.join(folder, i)))
        image = Image.open(os.path.join(folder, i))
        image = Image.Image.resize(image, [imageHeight, imageWidth])
        x = np.array(image)
        X.append(x)
        Y.append(str_to_num[name])


create_data(gnome_folder, "gnome")
create_data(drone_folder, "drone")

X_train, X_test, Y_train, Y_test = train_test_split(X, Y, test_size=0.1)

# Create Placeholders
X_place = tf.placeholder(tf.float32, shape=[None, imageHeight, imageWidth, 3])
Y_place = tf.placeholder(tf.int32, shape=[None, ])

one_hot = tf.one_hot(Y_place, 2)
one_hot = tf.cast(one_hot, tf.float32)

# Create the Network
input_layer = tf.reshape(X_place, shape=[-1, imageHeight, imageWidth, 3])

flatten = tf.reshape(input_layer, [-1, (imageHeight*imageWidth*3)])
fc1 = tf.layers.dense(flatten, units=1024, activation=tf.nn.relu)
fc2 = tf.layers.dense(fc1, units=1024, activation=tf.nn.relu)
fc3 = tf.layers.dense(fc2, units=1024, activation=tf.nn.relu)
dropout = tf.layers.dropout(fc1, rate=0.2)
logits = tf.layers.dense(dropout, units=2)

loss = tf.reduce_mean(tf.nn.softmax_cross_entropy_with_logits(logits=logits, labels=one_hot))
optimiser = tf.train.AdamOptimizer()
training_op = optimiser.minimize(loss)

correct_pred = tf.equal(tf.argmax(one_hot, 1), tf.argmax(logits, 1))
accuracy = tf.reduce_mean(tf.cast(correct_pred, tf.float32))

EPOCHS = 20
BATCH_SIZE = 10

# Run the Network
with tf.Session() as sess:
    sess.run(tf.global_variables_initializer())

    for each_epoch in range(EPOCHS):
        X_train, Y_train = shuffle(X_train, Y_train)

        for batch_start in range(0, len(X_train), BATCH_SIZE):
            batch_end = batch_start + BATCH_SIZE
            batch_X, batch_Y = X_train[batch_start:batch_end], Y_train[batch_start:batch_end]

            sess.run(training_op, feed_dict={X_place: batch_X, Y_place: batch_Y})
            train_accuracy = sess.run(accuracy, feed_dict={X_place: X_train, Y_place: Y_train})
            test_accuracy = sess.run(accuracy, feed_dict={X_place: X_test, Y_place: Y_test})

            print("\nEpoch: {}".format(each_epoch))
            print("Batch: {} out of {}".format(batch_start, len(X_train)))
            print("...")
            print("Train Accuracy: {a: 0.8f}".format(a=train_accuracy))
            print("Test Accuracy: {a: 0.8f}".format(a=test_accuracy))
