import styled from "styled-components";
import { useState, useEffect } from "react";
import axios from "axios";
import authService from "../api-authorization/AuthorizeService";

const Main = styled.div`
 // Fixar styling för profilsidan senare

`;

const Profile = () => {
    const [userData, setUserData] = useState("");

    useEffect(() => {
        const fetchData = async () => {
            try {
                const accessToken = await authService.getAccessToken();
                const headers = {
                    Authorization: `Bearer ${accessToken}`,
                };
                const response = await axios.get("/api/user/getuser", {
                    headers: headers,
                });
                console.log(response);
                setUserData(response.data);
            } catch (error) {
                console.error("Error fetching user data:", error);
            }
        };

        fetchData();
    }, []);

};
export default Profile;