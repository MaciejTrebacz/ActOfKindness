import {makeAutoObservable, reaction, runInAction} from "mobx";
import { toast } from 'react-toastify';
import {User} from "../models/Users/user";
import {LoginForm} from "../models/Users/loginForm";
import agent from "../api/agent";
import {router} from "../router/Routes";
import {store} from "./store";
import {RegisterForm} from "../models/Users/registerForm";

export default class AccountStore{
    token: string | null = localStorage.getItem('jwt')
    user: User | null = null
    isLoggedIn: boolean = false
    isAdmin: boolean = false
    isModerator: boolean = false
    redirectToLoginModal: boolean = false
    loadingUser: boolean = true

    constructor() {
        makeAutoObservable(this)
        // this reaction react only when token is changing no when is initialized
        reaction(()=>this.token,// what we want to react to
                token=>{// take argument(this.token)
            if (token){
                localStorage.setItem('jwt', token)
            } else{
                localStorage.removeItem('jwt')
            }
        })
    }

    login = async (loginForm: LoginForm)=>{
        try {
            const user = await agent.Account.login(loginForm)
            runInAction(()=>{
                this.setUser(user)
                this.setToken(user.token)
                this.isLoggedIn = true
                this.isAdmin = user.role === 'Admin'
                this.isModerator = user.role === 'Moderator'
                console.log(user)
            })
            await router.navigate(0)
            await router.navigate('/events')
            store.modalStore.closeModal()
        }
        catch (e) {
            throw e
        }
    }

    register = async (registerForm: RegisterForm)=>{
        try {
            const user = await agent.Account.register(registerForm)
            runInAction(()=>{
                this.setUser(user)
                this.setToken(user.token)
                this.isLoggedIn = true
                this.isAdmin = user.role === 'Admin'
                this.isModerator = user.role === 'Moderator'
                console.log(user)
            })
            await router.navigate('/events')
            store.modalStore.closeModal()
        }
        catch (e) {
            throw e;
        }
    }



    logout = async ()=>{
        this.setToken(null)
        this.user = null
        this.isLoggedIn = false
        this.isAdmin = false
        this.isModerator = false
        toast.success('Logged out successfully!');
        await router.navigate('/')
    }

    setToken = (token: string | null)=>{
        this.token = token
    }

    setUser = (user: User | null )=>{
        this.user = user
    }

    getUser = async ()=>{
        try {
            const user = await agent.Account.getCurrentUser();
            runInAction(()=>{
                this.setUser(user)
                this.isLoggedIn = true
                this.isAdmin = user.role === 'Admin'
                this.isModerator = user.role === 'Moderator'
                this.loadingUser = false
            });
        } catch (e){
            runInAction(()=>{
                this.loadingUser = false;
            });
            console.log("Error fetching user:", e);
        }
    }

    setMainImage = async(photoId: string)=>{
        if (this.user) this.user.mainPhotoUrl = photoId
    }

    setRedirectToLoginModal = (value: boolean)=>{
        this.redirectToLoginModal = value;
    }

    setLoadingUser = (value: boolean) => {
        this.loadingUser = value;
    }
}