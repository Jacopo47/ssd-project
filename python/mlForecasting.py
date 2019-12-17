# Sliding window MLP, Airline Passengers dataset (predicts t+1)
from __future__ import absolute_import, division, print_function, unicode_literals

import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
from flask import jsonify
from keras.layers import Dense
from keras.models import Sequential
import keras
from sklearn import preprocessing
import tensorflow as tf
import common

scaler = preprocessing.StandardScaler()
EPOCHS = 10
model = None


def univariate_data(dataset, start_index, end_index, history_size, target_size):
    data = []
    labels = []

    start_index = start_index + history_size
    if end_index is None:
        end_index = len(dataset) - target_size

    for i in range(start_index, end_index):
        indices = range(i - history_size, i)
        # Reshape data from (history_size,) to (history_size, 1)
        data.append(np.reshape(dataset.quant[indices.start: indices.stop].values, (history_size, 1)))
        labels.append(dataset.quant[i + target_size])
    return np.array(data), np.array(labels)


# from series of values to windows matrix
# questa funzione calcola la finestra
def compute_windows(nparray, npast=1):
    dataX, dataY = [], []  # window and value
    for i in range(len(nparray) - npast - 1):
        a = nparray[i:(i + npast), 0]
        dataX.append(a)
        dataY.append(nparray[i + npast, 0])
    return np.array(dataX), np.array(dataY)


def rnn_forecasting(model, scaler, df):
    TRAIN_SPLIT = len(df) - 3

    train = df[:-3]
    test = df[-3:]

    names = df.columns
    df = pd.DataFrame(scaler.fit_transform(df), columns=names)

    univariate_past_history = 12
    univariate_future_target = 0

    x_train, y_train = univariate_data(df, 0, TRAIN_SPLIT + 3, univariate_past_history, univariate_future_target)

    model.fit(x_train, y_train, epochs=EPOCHS)

    metrics = model.evaluate(x_train, y_train)
    
    metrics = {
        'mse': metrics[0],
        'mape': metrics[1],
        'mae': metrics[2]
    }

    forecast = model.predict(x_train)

    forecast = scaler.inverse_transform(forecast)

    return forecast, metrics, model, train, test


def rnn_forecasting_predict(model, scaler, path_to_sqlite, customer):
    df = common.load_orders(path_to_sqlite, "'" + customer + "'")
    TRAIN_SPLIT = len(df) - 3

    forecast, metrics, _, train, test = rnn_forecasting(model, scaler, df)

    # forecast = scaler.inverse_transform(forecast)

    # train = pd.Series(train)
    # test = pd.Series(test)

    data = []
    for i in range(TRAIN_SPLIT, TRAIN_SPLIT + 3):
        data.append([i, forecast[i - TRAIN_SPLIT]])

    df_forecast = pd.DataFrame(data, columns=['time', 'quant'])
    df_forecast = df_forecast.set_index('time')
    plt.figure()
    plt.plot(train, label='Train', color='blue')
    plt.plot(test, label='Test', color='green')
    plt.plot(df_forecast, label='Forecast', color='red')
    plt.legend()

    # last_forecast = forecasts[n_forecast - 1]
    # Finally, print the chart as base64 string to the console.
    # return common.print_figure(plt.gcf())

    returned_forecast = list()

    for i in df_forecast.quant.values:
        returned_forecast.append(i[0].item())

    return jsonify(
        metrics=metrics,
        forecasts=returned_forecast,
        image=common.get_figure(plt.gcf()).decode('utf-8')
    )


def rnn_forecasting_model(path_to_sqlite, scaler):
    np.random.seed(550)

    new_model = tf.keras.models.Sequential([
        tf.keras.layers.LSTM(8, input_shape=(12, 1)),
        tf.keras.layers.Dense(1)
    ])

    new_model.compile(optimizer='adam', loss='mse',
                      metrics=[keras.metrics.MAPE, keras.metrics.MAE])

    for i in range(1, 52):
        customer = "'cust" + str(i) + "'"
        df = common.load_orders(path_to_sqlite, customer)
        forecast, metrics, new_model, _, _ = rnn_forecasting(new_model, scaler, df)
        print(metrics)

    return new_model
