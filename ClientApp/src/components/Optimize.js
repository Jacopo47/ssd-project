import React, {Component} from 'react'
import Grid from "@material-ui/core/Grid";
import Container from "@material-ui/core/Container";
import DeleteIcon from '@material-ui/icons/Delete';
import ShowChartIcon from '@material-ui/icons/ShowChart';
import AddIcon from '@material-ui/icons/Add';
import ListItem from "@material-ui/core/ListItem";
import ListItemText from "@material-ui/core/ListItemText";
import List from "@material-ui/core/List";
import IconButton from "@material-ui/core/IconButton";
import ListItemSecondaryAction from "@material-ui/core/ListItemSecondaryAction";
import TextField from "@material-ui/core/TextField";
import Button from "@material-ui/core/Button";
import CircularProgress from "@material-ui/core/CircularProgress";
import {withSnackbar} from "notistack";
import {withStyles} from "@material-ui/core";

const useStyles = theme => ({
    container: {
        paddingTop: theme.spacing(4),
        paddingBottom: theme.spacing(4),
    },
    input: {
        margin: '2%'
    },
    error: {
        backgroundColor: '#d32f2f'
    }
});

class Optimize extends Component {

    constructor(props) {
        super(props);

        this.state = {
            loading: false,
            customer: '',
            iteration: 1000,
            tenure: 30,
            solution: 0,
            log: []
        };


        this.handleCustomerChange = this.handleCustomerChange.bind(this);
        this.handleIterationChange = this.handleIterationChange.bind(this);
        this.handleTenureChange = this.handleTenureChange.bind(this);
        this.localSearch = this.localSearch.bind(this);
        this.tabuSearch = this.tabuSearch.bind(this);
    }

    render() {
        const {classes} = this.props;

        const contents = this.state.loading ? (
            <CircularProgress color="secondary"/>
        ) : (
            <Grid container spacing={1}>
                <Grid item>
                    <h5>Solution -> {this.state.solution}</h5>
                    { this.state.log.map(e => {
                        return (<p>{e}</p>)
                    })}
                </Grid>
            </Grid>

        );
        

        return (
            <div>
                <h1>Optimize</h1>
                <Container maxWidth="lg" className={classes.container}>
                    <Grid container>
                        <Grid item xs={12}>
                            <div>
                                <TextField className={classes.input} id="customer-field" label="Customer" value={this.state.customer}
                                           onChange={e => this.handleCustomerChange(e)}/>
                                <TextField className={classes.input} id="iteration-field" label="TS Iterations" value={this.state.iteration}
                                           onChange={e => this.handleIterationChange(e)}/>
                                <TextField className={classes.input} id="tenure-field" label="TS Tenure" value={this.state.tenure}
                                           onChange={e => this.handleTenureChange(e)}/>
                                
                            </div>
                            <div>
                                <Button color="primary" aria-label="add customer"
                                            onClick={this.localSearch}>
                                    Local
                                </Button>
                                <Button color="primary" aria-label="add customer"
                                            onClick={this.tabuSearch}>
                                    Tabu search
                                </Button>
                            </div>
                        </Grid>
                        
                    </Grid>
                    <Grid container item>
                        <Grid item>
                            {contents}
                        </Grid>
                    </Grid>

                </Container>
            </div>
        )
    }

    async localSearch() {
        this.beforeFetch();

        fetch('api/optimize/' + this.state.customer + '/local')
            .then(response => {
                if (response.status !== 200) {
                    throw new Error('Response status -> ' + response.status)
                }

                response.json().then(data => {
                    const solution = data.solution;
                    const log = data.log;
                    this.setState({log: log, loading: false, solution: solution})
                }).catch(err => {
                    this.handleFetchDataFail(err.message)
                })
            })
            .catch(err => {
                this.handleFetchDataFail(err.message)
            });
    }

    async tabuSearch() {
        this.beforeFetch();

        fetch('api/optimize/' + this.state.customer + '/tabusearch?iterations=' + this.state.iteration + '&tenure=' + this.state.tenure)
            .then(response => {
                if (response.status !== 200) {
                    throw new Error('Response status -> ' + response.status)
                }

                response.json().then(data => {
                    const solution = data.solution;
                    const log = data.log;
                    this.setState({log: log, loading: false, solution: solution})
                }).catch(err => {
                    this.handleFetchDataFail(err.message)
                })
            })
            .catch(err => {
                this.handleFetchDataFail(err.message)
            });
    }

    beforeFetch() {
        this.setState({loading: true, log: [], solution: 0});
    }

    handleFetchDataFail(msgDetails) {
        this.props.enqueueSnackbar('Error on request: ' + msgDetails, {
            variant: 'error'
        });
        this.setState({ log: [], solution: 0, loading: false})
    }

    handleCustomerChange(e) {
        this.setState({
            customer: e.target.value
        })
    }
    handleIterationChange(e) {
        this.setState({
            iteration: e.target.value
        })
    }
    handleTenureChange(e) {
        this.setState({
            tenure: e.target.value
        })
    }
    
}


export default withStyles(useStyles)(
    withSnackbar(Optimize)
)