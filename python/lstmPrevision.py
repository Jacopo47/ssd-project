import pandas as pd, numpy as np, os
import matplotlib.pyplot as plt
from sklearn.preprocessing import MinMaxScaler
from keras.preprocessing.sequence import TimeseriesGenerator
from keras.models import Sequential
from keras.layers import Dense
from keras.layers import LSTM
import common


def lstm_prevision(path_to_sqlite, customer):
    plt.figure()

    df = common.load_orders(path_to_sqlite, "'" + customer + "'")

    plt.title('LSTM forecast - ' + customer, color='black')

    aSales = df['quant'].to_numpy()

    # array of sales data
    logdata = np.log(aSales)

    # log transform
    data = pd.Series(logdata)

    # data plot
    # train and test set
    train = data[:-3]
    test = data[-3:]

    # ------------------------------------------------- neural forecast

    scaler = MinMaxScaler()
    scaler.fit_transform(train.values.reshape(-1, 1))
    scaled_train_data = scaler.transform(train.values.reshape(-1, 1))
    scaled_test_data = scaler.transform(test.values.reshape(-1, 1))

    n_input = 12
    n_features = 1
    generator = TimeseriesGenerator(scaled_train_data, scaled_train_data,
                                    length=n_input, batch_size=1)

    lstm_model = Sequential()
    lstm_model.add(LSTM(20, activation='relu', input_shape=(n_input, n_features),
                        dropout=0.05))
    lstm_model.add(Dense(1))
    lstm_model.compile(optimizer='adam', loss='mse')
    lstm_model.summary()
    lstm_model.fit_generator(generator, epochs=25)

    test_generator = TimeseriesGenerator(scaled_train_data, scaled_train_data, length=12, batch_size=1)
    result_evalute = lstm_model.evaluate_generator(test_generator)
    print(result_evalute)

    plt.xticks(np.arange(0, 21, 1))

    lstm_predictions_scaled = list()
    batch = scaled_train_data[-n_input:]
    curbatch = batch.reshape((1, n_input, n_features))

    for i in range(len(test)):
        lstm_pred = lstm_model.predict(curbatch)[0]
        lstm_predictions_scaled.append(lstm_pred)
        curbatch = np.append(curbatch[:, 1:, :], [[lstm_pred]], axis=1)

    lstm_forecast = scaler.inverse_transform(lstm_predictions_scaled)
    yfore = np.transpose(lstm_forecast).squeeze()

    # recostruction
    expdata = np.exp(train)  # unlog
    exptest = np.exp(test)
    expfore = np.exp(yfore)
    plt.plot(df.quant, label="Train", color='yellow')
    plt.plot(exptest, label='Test', color='purple')
    plt.plot([None for x in expdata] + [x for x in expfore], label='forecast', color='blue')
    plt.legend()
    plt.show()


lstm_prevision('../ordiniMI2018.sqlite', 'cust40')
