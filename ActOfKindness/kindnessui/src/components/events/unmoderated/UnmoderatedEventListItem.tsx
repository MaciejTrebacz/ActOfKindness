import {MyEvent} from "../../../app/models/Events/myEvent";
import {Button, Grid, Icon, Item, Label, Segment} from "semantic-ui-react";
import {observer} from "mobx-react-lite";
import { Link } from 'react-router-dom';
import { format } from 'date-fns';
import Swal from 'sweetalert2';
import {useStore} from "../../../app/stores/store";

interface Props {
    event: MyEvent
}

function UnmoderatedEventListItem({event}: Props) {
    const { eventStore } = useStore();
    const { moderateEvent, deleteEvent } = eventStore;

    return (
        <Segment.Group style={{marginBottom:40}}  >
            <Segment className={"eventDashboard"} >
                <Item.Group relaxed divided>
                    <Item>
                        <Item.Image size='medium' src={event.image} />
                        <Item.Content>
                            <Label style={{margin:4,padding:6}} as='a' color='red' ribbon={"right"}>
                                {event.type === 0 ? "Help Ask" : "Help Offer"}
                            </Label>
                            <Item.Header>{event.title}</Item.Header>
                            <Item.Description>{event.description}</Item.Description>
                            <Item.Extra>{event.localization}</Item.Extra>
                        </Item.Content>
                    </Item>
                </Item.Group>
            </Segment>

            <Segment clearing className={"eventDashboard"} >
                <Grid>
                    <Grid.Column width={4}>
                        <span>
                        <Icon name='calendar' style={{marginBottom: 10}} size='large' color='teal'/>
                            {event.startingDate && format(new Date(event.startingDate), "dd/MM/yyyy")} - {event.endingDate && format(new Date(event.endingDate), "dd/MM/yyyy")}
                        </span>
                    </Grid.Column>

                    <Grid.Column width={3}>
                        <span>
                        <Icon name='location arrow' style={{marginBottom: 10}} size='large' color='teal'/>
                            {event.localization}
                        </span>
                    </Grid.Column>

                    <Grid.Column width={3}>
                        <span>
                        <Icon name='user' style={{marginBottom: 10}} size='large' color='teal'/>
                            <Link to={`/profile/${event.createdBy.username}`}>{event.createdBy.nickname}</Link>
                        </span>
                    </Grid.Column>

                    <Grid.Column width={6}>
                        <Button as={Link}
                            floated={"right"}
                            onClick={async () => {
                                const result = await Swal.fire({
                                    title: 'Are you sure?',
                                    text: "You won't be able to revert this!",
                                    icon: 'warning',
                                    showCancelButton: true,
                                    confirmButtonColor: 'red',
                                    confirmButtonText: 'Yes, delete it!',
                                    cancelButtonText: 'No, cancel!',
                                    background: '#1b1c1d'
                                })
                                if (result.isConfirmed) {
                                    deleteEvent(event.id)
                                }
                            }}
                            color={"red"}
                            content={"Delete"}
                            style={{marginLeft: 10}}
                        ></Button>

                        <Button as={Link}
                                floated={"right"}
                                to={`/editEvent/${event.id}`}
                                color={"orange"}
                                content={"Edit"}
                                style={{marginLeft: 10}}
                        ></Button>

                        <Button as={Link}
                            floated={"right"}
                            onClick={async () => {
                                const result = await Swal.fire({
                                    title: 'Are you sure?',
                                    text: "You are about to moderate this event!",
                                    icon: 'warning',
                                    showCancelButton: true,
                                    confirmButtonColor: 'teal',
                                    confirmButtonText: 'Yes, moderate it!',
                                    cancelButtonText: 'No, cancel!',
                                    background: '#1b1c1d'
                                })
                                if (result.isConfirmed) {
                                    moderateEvent(event.id)
                                }
                            }}
                            color={"teal"}
                            content={"Moderate"}
                        ></Button>
                    </Grid.Column>

                </Grid>
            </Segment>
        </Segment.Group>
    );
}

export default observer(UnmoderatedEventListItem);