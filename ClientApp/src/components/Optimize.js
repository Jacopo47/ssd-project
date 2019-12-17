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

class Optimize extends Component {

    constructor(props) {
        super(props);

        this.state = {
            
        };
        
    }

    render() {
        

        return (
            <div>
                <h1>Optimize</h1>
            </div>
        )
    }
    
}


export default withStyles(useStyles)(
    withSnackbar(Optimize)
)