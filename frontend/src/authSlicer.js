import { createSlice } from "@reduxjs/toolkit";

const authSlice = createSlice({
  name: "auth",
  initialState: {
    username: null,
    role: null, // Add role to state
    employeeId: null,
  },
  reducers: {
    login: (state, action) => {
      console.log(action.payload);
      state.username = action.payload.username;
      state.role = action.payload.role; // Set role
      state.employeeId = action.payload.employeeId;
    },
    logout: (state) => {
      state.username = null;
      state.role = null; // Clear role on logout
      state.employeeId = null;
    },
  },
});

export const { login, logout } = authSlice.actions;
export default authSlice.reducer;