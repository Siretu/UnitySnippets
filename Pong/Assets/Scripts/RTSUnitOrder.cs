/*
 * Name: RTS Unit Order
 * Author: James 'Sevion' Nhan
 * Date: 04/07/2013
 * Version: 1.0.0.0
 * Description:
 *		This is a simple RTS unit order system that
 *		handles orders on mouse clicks.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RTSUnitOrder : MonoBehaviour {
    // The possible orders
    public enum Order {
        Move,
        Stop,
        // Still need to implement:
        HoldPosition,
        Patrol,
        Attack,
        Smart
    };

    // OrderStruct for issuing orders
    public struct OrderStruct {
        public Order order;
        public Vector3 target;

        public OrderStruct(Order order, Vector3 target) {
            this.order = order;
            this.target = target;
        }
    };

    private Order CurrentOrder = Order.Stop;
    private GameObject TargetObject;
    private Vector3 TargetPosition;
    private const float MOVESPEED = 5.0f;

    void Update() {
        if (CurrentOrder == Order.Move) {
            // This is the new position with respect to time
            Vector3 NewPosition = (TargetPosition - gameObject.transform.position).normalized * MOVESPEED * Time.deltaTime;
            // Clamping
            if (Vector3.Distance(gameObject.transform.position, TargetPosition) > MOVESPEED * Time.deltaTime) {
                gameObject.transform.position += NewPosition;
            } else {
                CurrentOrder = Order.Stop;
                gameObject.transform.position = TargetPosition;
            }
        }
    }

    // Issue an order to the unit
    public void IssueOrder(OrderStruct order) {
        // If it's either of the non-motion orders, there shouldn't be a target
        if (order.order == Order.Stop || order.order == Order.HoldPosition) {
            CurrentOrder = order.order;
            TargetPosition = gameObject.transform.position;
            TargetObject = gameObject;
        } else {
            CurrentOrder = order.order;
            // Make sure the target doesn't cause the unit to "sink" in to the ground
            order.target += new Vector3(0, gameObject.transform.position.y, 0);
            TargetPosition = order.target;
        }
    }

    public Order GetCurrentOrder() {
        return CurrentOrder;
    }

    public Vector3 GetOrderPosition() {
        return TargetPosition;
    }

    public GameObject GetOrderTarget() {
        return TargetObject;
    }
}
