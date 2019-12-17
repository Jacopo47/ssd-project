# Sliding window MLP, Airline Passengers dataset (predicts t+1)
import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
from keras.layers import Dense
from keras.models import Sequential
import common


# from series of values to windows matrix
# questa funzione calcola la finestra
def compute_windows(nparray, npast=1):
    dataX, dataY = [], []  # window and value
    for i in range(len(nparray) - npast - 1):
        a = nparray[i:(i + npast), 0]
        dataX.append(a)
        dataY.append(nparray[i + npast, 0])
    return np.array(dataX), np.array(dataY)


def sliding_window_forecasting(path_to_sqlite, customer):
    np.random.seed(550)
    # for reproducibility
    df = common.load_orders(path_to_sqlite, "'" + customer + "'")
    dataset = df.values
    # time series values
    dataset = dataset.astype('float32')  # needed for MLP input

    # train - test sets
    cutpoint = 3
    # 70% train, 30% test
    train, test = dataset[:-cutpoint], dataset[-cutpoint:]
    print("Len train={0}, len test={1}".format(len(train), len(test)))
    # sliding window matrices (npast = window width); dim = n - npast - 1
    npast = 3
    # trainX, trainY = compute_windows(train, npast)
    # testX, testY = compute_windows(test, npast)  # should get also the last npred of train

    windowX, windowY = compute_windows(dataset, npast)

    trainX, trainY = windowX[:-cutpoint], windowY[:-cutpoint]
    testX, testY = windowX[-cutpoint:], windowY[-cutpoint:]

    # Multilayer Perceptron model
    model = Sequential()
    n_hidden = 8  # neuroni strato nascosto

    n_output = 1  # neuroni di output

    # Da qui costruisco la rete

    # Aggiungo allo strato di input uno strato Dense (che vuol dire tutti i neuroni collegati tra loro)
    model.add(Dense(n_hidden, input_dim=npast, activation='relu'))  # hidden neurons, 1 layer

    # Aggiungo uno strato alla rete, questo è lo stato di uscita (Dense vuol dire che tutti i neuroni di questo stato
    # sono collegati a tutti quelli prima)
    model.add(Dense(n_output))  # output neurons

    # Compilo il modello, effettivamente istanzio la rete in memoria, dice che dovrà addestrare la rete cercando di
    # minimizzare la funzione di errore. La loss function scelta è mean_squared_error Optimizer -> adam è un
    # ottimizzatore gredy ottimizzato
    model.compile(loss='mean_squared_error', optimizer='adam')

    # Model.fit è l'istruzione che lancia effettivamente backpropagation sulla rete. Da qui inizio ad apprendere Epochs
    # sono il numero di iterazioni da fare Batch_size partiziona il TS e propone ogni volta di apprendere su un insieme
    # di record (in poche parole utile per la parallelizzazione)
    model.fit(trainX, trainY, epochs=200, batch_size=1, verbose=2)  # batch_size divisor of

    # Model performance
    # Si calcola l'errore sul Training SET
    trainScore = model.evaluate(trainX, trainY, verbose=0)
    print('Score on train: MSE = {0:0.2f} '.format(trainScore))
    # Si calcola l'errore sul Test SET
    testScore = model.evaluate(testX, testY, verbose=0)
    print('Score on test: MSE = {0:0.2f} '.format(testScore))

    trainPredict = model.predict(trainX)  # predictions

    # Il predict sul test ci fornisce i dati di forecast
    testForecast = model.predict(testX)  # forecast

    plt.rcParams["figure.figsize"] = (10, 8)  # redefines figure size
    # plt.plot(train, label='Training set', color='blue')  # blu
    # plt.plot(test, label='Test set', color='green')
    plt.plot(dataset, label='Dataset')
    plt.plot(np.concatenate((np.full(1, np.nan), trainPredict[:, 0])), label='Train predict', color='orange')  # orange
    plt.plot(np.concatenate((np.full(len(train) + 1, np.nan), testForecast[:, 0])), label='Forecast',
             color='red')  # green
    plt.legend()
    plt.show()


sliding_window_forecasting('../ordiniMI2018.sqlite', 'cust40')
