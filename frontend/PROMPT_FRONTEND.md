# Frontend Development via AI Prompting

This document outlines the specific prompt used to generate the Dashboard and the initial frontend architecture for this solution, ensuring full alignment with the .NET backend.

## Prompt Definition

**Tool Used:** Claude 4.6  
**Assigned Role:** Senior Fullstack Developer

### Message Body
> "Act as a Senior Fullstack Developer expert in Vue 3"
> 
> **Project Context:**
> You can read all my code to create a custom solution.
> 
> **Mission:**
> Create a high-level main Dashboard with a professional visual style (RSM style: sober, navy blue, slate gray, and white colors, clean typography).
> 
> **Dashboard Components:**
> - **KPI Cards:** 4 top cards showing: Total People, Total Products, and two calculated metrics based on the data retrieved from the API.
> - **Data Visualization:** A chart component showing the registration trend.
> - **Global Search Bar:** An intelligent input that filters both the people and product lists.
> - **Data Tables:** Implement tables for 'people' and 'products' integrating the pagination I already have in the backend (page, pageSize, totalCount).
> 
> **RSM Style:**
> - Sidebar design for navigation.
> - Responsive layout (Mobile Friendly).
> - Tables with hover states and action buttons (Edit/Delete)."
