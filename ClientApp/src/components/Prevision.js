import React, {Component} from 'react'
import Grid from "@material-ui/core/Grid";
import Container from "@material-ui/core/Container";
import DeleteIcon from '@material-ui/icons/Delete';
import ShowChartIcon from '@material-ui/icons/ShowChart';
import AddIcon from '@material-ui/icons/Add';
import DeviceHubIcon from '@material-ui/icons/DeviceHub';
import ListItem from "@material-ui/core/ListItem";
import ListItemText from "@material-ui/core/ListItemText";
import List from "@material-ui/core/List";
import IconButton from "@material-ui/core/IconButton";
import ListItemSecondaryAction from "@material-ui/core/ListItemSecondaryAction";
import TextField from "@material-ui/core/TextField";
import Button from "@material-ui/core/Button";
import CircularProgress from "@material-ui/core/CircularProgress";
import MultilineChartIcon from '@material-ui/icons/MultilineChart';
import {withSnackbar} from "notistack";
import {withStyles} from "@material-ui/core";
import Tooltip from '@material-ui/core/Tooltip';

const useStyles = theme => ({
    container: {
        paddingTop: theme.spacing(4),
        paddingBottom: theme.spacing(4),
    },
    customersList: {
        width: '100%',
        maxWidth: 360,
        backgroundColor: theme.palette.background.paper,
    },
    imageContainer: {
        width: '80%'
    },
    error: {
        backgroundColor: '#d32f2f'
    }
});

class Prevision extends Component {

    constructor(props) {
        super(props);

        this.state = {
            imageAsString: undefined,
            loading: false,
            metrics: undefined,
            customers: [],
            newCustomer: ''
        };

        this.handleCustomerChange = this.handleCustomerChange.bind(this);
        this.addCustomer = this.addCustomer.bind(this);
        this.removeCustomer = this.removeCustomer.bind(this);
        this.getImage = this.getImage.bind(this);
        this.handleFetchDataFail = this.handleFetchDataFail.bind(this);
    }

    render() {
        const {classes} = this.props;

        const metricsData = this.getMetricsInfo();

        const contents = this.state.loading ? (
            <CircularProgress color="secondary"/>
        ) : (
            <Grid container spacing={1}>
                <Grid item xs={10}>
                    <div>
                        <img src={`data:image/jpeg;base64,${this.state.imageAsString}`} alt='Prevision'
                             className={classes.imageContainer}/>
                    </div>
                </Grid>
                <Grid item xs={2}>
                    {metricsData}
                </Grid>
            </Grid>
            
        );

        return (
            <div>
                <h1>Prevision</h1>
                <Container maxWidth="lg" className={classes.container}>
                    <Button onClick={this.getImage} variant="outlined" color="primary">
                        Show orders
                    </Button>
                    <Grid container spacing={3}>
                        <Grid item xs={4}>
                            <div>
                                <TextField id="new-customer-field" label="New customer" value={this.state.newCustomer}
                                           onChange={e => this.handleCustomerChange(e)}/>
                                <IconButton color="primary" aria-label="add customer"
                                            onClick={this.addCustomer}>
                                    <AddIcon/>
                                </IconButton>
                            </div>
                            <h6>Customers:</h6>
                            {this.customersList()}
                        </Grid>
                        <Grid item xs={8}>
                            {contents}
                        </Grid>
                    </Grid>

                </Container>


            </div>
        )
    }

    customersList() {
        const {classes} = this.props;

        return (
            <List component="nav" className={classes.customersList} aria-label="contacts">
                {
                    this.state.customers.map(customer => {
                        return (
                            <ListItem key={customer}>
                                <ListItemText primary={customer}/>
                                <ListItemSecondaryAction>
                                    <Tooltip title="Arima forecasting">
                                        <IconButton edge="end" aria-label="delete" size="small"
                                                    onClick={() => this.onSingleCustomerPrevisionArima(customer)}>
                                            <ShowChartIcon/>
                                        </IconButton>
                                    </Tooltip>
                                    <Tooltip title="Sarimax forecasting">
                                        <IconButton edge="end" aria-label="delete" size="small"
                                                    onClick={() => this.onSingleCustomerPrevisionSarimax(customer)}>
                                            <MultilineChartIcon/>
                                        </IconButton>
                                    </Tooltip>
                                    <Tooltip title="Lstm Neural Network forecasting">
                                        <IconButton edge="end" aria-label="delete" size="small"
                                                    onClick={() => this.onSingleCustomerPrevisionLstmNeuralNetwork(customer)}>
                                            <DeviceHubIcon/>
                                        </IconButton>
                                    </Tooltip>
                                    <IconButton edge="end" aria-label="delete"
                                                onClick={() => this.removeCustomer(customer)}>
                                        <DeleteIcon/>
                                    </IconButton>
                                </ListItemSecondaryAction>
                            </ListItem>
                        )
                    })
                }
            </List>
        )
    }

    handleCustomerChange(e) {
        this.setState({
            newCustomer: e.target.value
        })
    }

    addCustomer() {
        const newCustomer = this.state.newCustomer;

        if (newCustomer === '') return;

        const newCustomersList = this.state.customers;


        const isDuplicate = newCustomersList.filter(e => e === newCustomer).length;

        if (isDuplicate > 0) {
            return;
        }

        newCustomersList.push(this.state.newCustomer);

        this.setState({
            customers: newCustomersList,
            newCustomer: ''
        })
    }

    removeCustomer(customer) {
        const customers = this.state.customers.filter(e => e !== customer);

        this.setState({
            customers
        })
    }

    getMetricsInfo() {
        const {classes} = this.props;

        const metrics = this.state.metrics;
        
        return metrics ? (
            <List component="nav" className={classes.customersList} aria-label="contacts">
                {
                    Object.keys(metrics).map(key => {
                        const value = metrics[key];
                        
                        if (value === undefined || isNaN(value)) {
                            return null;
                        }
                        
                        return (
                            <ListItem key={key}>
                                <ListItemText primary={metrics[key].toFixed(2)} secondary={key.toUpperCase()}/>
                            </ListItem>
                        )
                    })
                }
                
            </List>
        ) : null;

    }

    async onSingleCustomerPrevisionArima(customer) {
        this.beforeFetch();

        fetch('api/prevision/' + customer + '/arima')
            .then(response => {
                if (response.status !== 200) {
                    throw new Error('Response status -> ' + response.status)
                }

                response.json().then(data => {
                    const lastValues = data.forecasts;
                    let counter = 1;
                    lastValues.forEach(e => {
                        this.props.enqueueSnackbar(counter + '^ forecast value -> ' + e.toFixed(2));
                        counter++;
                    });
                    this.setState({imageAsString: data.image, loading: false, metrics: data.metrics})
                }).catch(err => {
                    this.handleFetchDataFail(err.message)
                })
            })
            .catch(err => {
                this.handleFetchDataFail(err.message)
            });
    }

    async onSingleCustomerPrevisionSarimax(customer) {
        this.beforeFetch();

        fetch('api/prevision/' + customer + '/sarimax')
            .then(response => {
                if (response.status !== 200) {
                    throw new Error('Response status -> ' + response.status)
                }

                response.json().then(data => {
                    const lastValues = data.forecasts;
                    let counter = 1;
                    lastValues.forEach(e => {
                        this.props.enqueueSnackbar(counter + '^ forecast value -> ' + e.toFixed(2));
                        counter++;
                    });
                    this.setState({imageAsString: data.image, loading: false, metrics: data.metrics})
                }).catch(err => {
                    this.handleFetchDataFail(err.message)
                })
            })
            .catch(err => {
                this.handleFetchDataFail(err.message)
            });
    }

    async onSingleCustomerPrevisionLstmNeuralNetwork(customer) {
        this.beforeFetch();

        fetch('api/prevision/' + customer + '/ml')
            .then(response => {
                if (response.status !== 200) {
                    throw new Error('Response status -> ' + response.status)
                }

                response.json().then(data => {
                    const lastValues = data.forecasts;
                    let counter = 1;
                    lastValues.forEach(e => {
                        this.props.enqueueSnackbar(counter + '^ forecast value -> ' + e.toFixed(2));
                        counter++;
                    });
                    this.setState({imageAsString: data.image, loading: false, metrics: data.metrics})
                }).catch(err => {
                    this.handleFetchDataFail(err.message)
                })
            })
            .catch(err => {
                this.handleFetchDataFail(err.message)
            });
    }


    async getImage() {
        this.beforeFetch();

        fetch('api/prevision', {
            method: 'POST',
            body: JSON.stringify(this.state.customers),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => {
                if (response.status !== 200) {
                    throw new Error('Response status -> ' + response.status)
                }

                response.json().then(data => {
                    this.setState({imageAsString: data, loading: false})
                }).catch(err => {
                    this.handleFetchDataFail(err.message)
                })
            })
            .catch(err => {
                this.handleFetchDataFail(err.message)
            });
    }

    beforeFetch() {
        this.setState({loading: true, metrics: undefined});
    }


    handleFetchDataFail(msgDetails) {
        this.props.enqueueSnackbar('Error on request: ' + msgDetails, {
            variant: 'error'
        });
        this.setState({imageAsString: undefined, loading: false})
    }
}


export default withStyles(useStyles)(
    withSnackbar(Prevision)
)